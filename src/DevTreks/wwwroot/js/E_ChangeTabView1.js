/*
    Name:ChangeTabView.js
    Author: www.devtreks.org
    Date: 2012, September
    Purpose: set up tabs, respond to tab events using jquery
    called using syntax: var oChangeTabViewer = new ChangeTabView.ViewChanger(params);
*/
//first script loaded
function changeLinkedViewsTabs(){
    var oChangeTabViewer = new ChangeTabView.ViewChanger(null, "linkedviews");
}
//constructors
var ChangeMvcTabView = new Object();
ChangeMvcTabView.ViewChanger=function(){
   var oChangeTabViewer = new ChangeTabView.ViewChanger(null);
   //HighlightLinkClicked(id);
}
var ChangeTabView = new Object();
ChangeTabView.ViewChanger=function(onError, tabId, updatePanelType){
    this.onError = (onError) ? onError : this.defaultError;
    if (arguments.length <= 1){
        //window.onload (no click event)
		this.initTabs();		
	}
    else if (arguments.length == 2) {
        //error messages are cleared by clicking on any tab
        ClearErrorMessage();
	    //tab click event
	    this.changeTabs(tabId);
	}
	else if (arguments.length == 3){
	    //other click event
	    this.changeTabsByUpdatePanelType(updatePanelType);
	}
	//else: an event on this class has been raised (i.e. callback events from content loaders)
}
//object literal
ChangeTabView.ViewChanger.prototype={
    initTabs:function(){
        //check for serveraction in uri: www.devtreks.org/agtreks/search
        var sURI = window.location.href;
        //menu id has # symbol while corresponding div does not
        var sTabId = "menusearch";
        if (sURI) {
            var bHasGoodTab = false;
            var sServerActionType = GetSubStringWithDelimiter(sURI, SharedUtils.URIPatternDelimiter, 4);
            if (sServerActionType != null && sServerActionType != undefined){
                var regServerActionType = new RegExp("search");
                if (regServerActionType.test(sServerActionType)){
                    sTabId = "menusearch";
                    bHasGoodTab = true;
                }
                if (bHasGoodTab == false) {
                    regServerActionType = new RegExp("preview");
                    if (regServerActionType.test(sServerActionType)){
                        sTabId = "menupreview";
                        bHasGoodTab = true;
                    }
                }
                if (bHasGoodTab == false) {
                    regServerActionType = new RegExp("select");
                    if (regServerActionType.test(sServerActionType)){
                        sTabId = "menuselect";
                        bHasGoodTab = true;
                    }
                }
                if (bHasGoodTab == false) {
                    regServerActionType = new RegExp("edit");
                    if (regServerActionType.test(sServerActionType)){
                        sTabId = "menuedit";
                        bHasGoodTab = true;
                    }
                }
                if (bHasGoodTab == false) {
                    regServerActionType = new RegExp("pack");
                    if (regServerActionType.test(sServerActionType)){
                        sTabId = "menupack";
                        bHasGoodTab = true;
                    }
                }
                if (bHasGoodTab == false) {
                    regServerActionType = new RegExp("linkedviews");
                    if (regServerActionType.test(sServerActionType)){
                        sTabId = "menulinkedviews";
                        bHasGoodTab = true;
                    }
                }
                if (bHasGoodTab == false) {
                    regServerActionType = new RegExp("member");
                    if (regServerActionType.test(sServerActionType)){
                        sTabId = "menumember";
                        bHasGoodTab = true;
                    }
                }
            }
        }
        else {
            //search is default
            sTabId = "menusearch";
        }
        this.changeTabs(sTabId);
    },//initTabs
    changeTabsByUpdatePanelType:function(updatePanelType){
        var sTabId = this.getTabIdFromUpdatePanelType(updatePanelType);
        this.changeTabs(sTabId);
    },//changeTabsByUpdatePanelType
    changeTabs:function(tabId){
        var sTabId = '#' + tabId;
        //toggle the appearance of the selected linkui-btn-active ui-state-persist
        //show the corresponding selected div (i.e. menusearch = divsearch)
        var sPanelId = "";
        if (StartsWith("menu", tabId)){
            $("a[id^='menu']").removeClass("ui-btn-active");
            $(sTabId).addClass("ui-btn-active");
            //this works identically
            //$("a[id^='menu']").removeClass($.mobile.activeBtnClass);
            //$(sTabId).addClass($.mobile.activeBtnClass);
            $("#basecontent").children("div").hide();
            sPanelId = ReplaceString(tabId, "menu", "");
            $('#div' + sPanelId).toggle();
        }
        else if (StartsWith("step", tabId)){
            $("a[id^='step']").removeClass("ui-btn-active");
            $(sTabId).addClass("ui-btn-active");
            sPanelId = '#div' + tabId;
            $(sPanelId).siblings("div").hide();
            $(sPanelId).hide();
            $(sPanelId).toggle();
            $("#" + "stepsmenu").toggle();
        }
        //show the message div
        $("#spanDisplayError").show();
        //the views tab uses multi-step views (i.e. calculator steps)
        this.initSecondTabs(tabId);
    },//changeTabs
    initSecondTabs:function(tabId){
        if (tabId){
            if (EndsWith("linkedviews", tabId)) {
                //init tab on instructions/intro panel
                var sStepId;
                if (SharedUtils.TabId != null){
                    sStepId = SharedUtils.TabId;
                }
                else {
                    sStepId = "stepzero";
                }
                this.changeTabs(sStepId);
            }
            //persist it if a step
            if (tabId.indexOf("step") > -1) {
                SharedUtils.TabId = tabId;
            }
        }
    },//initSecondTabs
     initThirdTabs:function(tabId){
        //refactor: 1. multiple docs with same menu (dynamic tabs may be needed for each menu)
        //          2. respective stylesheets need standard id attribute values (of some type)
        //          3. animation : show the div expanding to 100% while the previous one contracts 
        var regIsSectionTab = new RegExp("^section");
        if (!regIsSectionTab.test(tabId)){
            var thirdTabbedMenuItem = $("#sectionsmenu");
            if ($(thirdTabbedMenuItem).length > 0) {
                //init sections in documents (refactor for multiple docs)
                this.changeTabs("section0");
            }
        }
    },//initThirdTabs
    getTabIdFromUpdatePanelType:function(updatePanelType){
        var sTabId = "menusearch";
        updatePanelType = this.getPanelAction(updatePanelType);
        switch(updatePanelType){
            case SharedUtils.UPDATE_PANEL_TYPES_search:
                sTabId = "menusearch";
                break;
            case SharedUtils.UPDATE_PANEL_TYPES_preview:
                sTabId = "menupreview";
                break;
            case SharedUtils.UPDATE_PANEL_TYPES_select:
                sTabId = "menuselect";
                break;
            case SharedUtils.UPDATE_PANEL_TYPES_edit:
                sTabId = "menuedit";
                break;
            case SharedUtils.UPDATE_PANEL_TYPES_pack:
                sTabId = "menupack";
                break;
            case SharedUtils.UPDATE_PANEL_TYPES_linkedviews:
                sTabId = "menulinkedviews";
                break;
            case SharedUtils.UPDATE_PANEL_TYPES_member:
                sTabId = "menumember";
                break;
            default:
                break;
        }
        return sTabId;
    },//getTabIdFromUpdatePanelType
    getPanelAction:function(updatePanelType) {
        //cross browser support
        var ePanelType;
        if (isNaN(updatePanelType)) {
            if (updatePanelType == "SharedUtils.UPDATE_PANEL_TYPES_search"){
                ePanelType = SharedUtils.UPDATE_PANEL_TYPES_search;
            }
            else if (updatePanelType == "SharedUtils.UPDATE_PANEL_TYPES_preview"){
                ePanelType = SharedUtils.UPDATE_PANEL_TYPES_preview;
            }
            else if (updatePanelType == "SharedUtils.UPDATE_PANEL_TYPES_edit"){
                ePanelType = SharedUtils.UPDATE_PANEL_TYPES_edit;
            }
            else if (updatePanelType == "SharedUtils.UPDATE_PANEL_TYPES_select"){
                ePanelType = SharedUtils.UPDATE_PANEL_TYPES_select;
            }
            else if (updatePanelType == "SharedUtils.UPDATE_PANEL_TYPES_pack"){
                ePanelType = SharedUtils.UPDATE_PANEL_TYPES_pack;
            }
            else if (updatePanelType == "SharedUtils.UPDATE_PANEL_TYPES_member"){
                ePanelType = SharedUtils.UPDATE_PANEL_TYPES_member;
            }
            else if (updatePanelType == "SharedUtils.UPDATE_PANEL_TYPES_linkedviews"){
                ePanelType = SharedUtils.UPDATE_PANEL_TYPES_linkedviews;
            }
            else if (updatePanelType == "SharedUtils.UPDATE_PANEL_TYPES_preview"){
                ePanelType = SharedUtils.UPDATE_PANEL_TYPES_preview;
            }
        }
        else {
            ePanelType = updatePanelType;
        }
        return ePanelType;
    },
    hideAfterDeletes:function(){
        //hide other divs that might still contain the deleted element
        $('#divsearch').hide();
        $('#divselect').hide();
        $('#divpreview').hide();
    },
    defaultError:function(){
        alert("couldn't change tab view");
    }
}
