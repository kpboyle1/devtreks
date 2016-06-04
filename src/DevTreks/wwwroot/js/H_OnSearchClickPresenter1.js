
/*
    Name:OnSearchClickPresenter.js
    Author: www.devtreks.org
    Date: 2011, November
    Purpose: handler for search page onclick events and methods
    Notes:  To keep all event and method handling consistent,
            this class is always initialized using the 
            OnAjaxClickPresenter.ChangePresentation() object.
            
*/
//devtreks standard search onclick method
function searchDevTreksOnClick(contenturipattern, clientaction, params) {
    //search click events
    var presentationChanger = new OnAjaxClickPresenter.ChangePresentation(
        contenturipattern, clientaction, params);
    var searchChanger = new OnSearchAjaxClickPresenter.ChangePresentation(presentationChanger);
    searchChanger.onClick();
}
//object constructor
var OnSearchAjaxClickPresenter = new Object();
OnSearchAjaxClickPresenter.ChangePresentation=function(ajaxPresentationChanger){
    if (ajaxPresentationChanger != null && ajaxPresentationChanger != undefined){
        //the search params either come from args or page dom elements
        this.contenturipattern = ajaxPresentationChanger.contenturipattern;
        this.controller = ajaxPresentationChanger.controller;
        //this object always carries out searches
        this.serveraction = "search";
        this.network = ajaxPresentationChanger.network;
        this.node = ajaxPresentationChanger.node;
        this.name = ajaxPresentationChanger.name;
        this.id = ajaxPresentationChanger.id;
        this.fileextension = ajaxPresentationChanger.fileextension;
        this.subaction = ajaxPresentationChanger.subaction;
        this.subactionview = ajaxPresentationChanger.subactionview;
        this.variable = ajaxPresentationChanger.variable;
        this.clientaction = ajaxPresentationChanger.clientaction;
        this.params = ajaxPresentationChanger.params;
    }
}
//encapsulate object props, methods, events in an object literal
OnSearchAjaxClickPresenter.ChangePresentation.prototype = {
    onClick: function () {
        //init this.object specific properties
        this.initParams();
        var sProgressId = GetProgressId(this.clientaction);
        var eUpdatePanelType = SharedUtils.UPDATE_PANEL_TYPES_search;
        switch (this.clientaction) {
            //keep this pattern consistent with contentchanger for future use             
            case SharedUtils.CLIENTACTION_postrequest:
                eUpdatePanelType = SharedUtils.UPDATE_PANEL_TYPES_search;
                break;
            default:
                break;
        }
        this.sendRequest(sProgressId);
    },
    initParams: function () {
        //this specific
        this.searchName = this.getSearchName();
        this.networkFilterType = this.getNetworkFilterType();
        this.networkId = this.getNetworkId();
        this.serviceId = this.getServiceId();
        this.currentNodeName = this.getNodeName();
        this.typeId = this.getTypeId(this.currentNodeName);
        this.clientaction = SharedUtils.CLIENTSEARCHACTION_search;
        this.subaction = this.getServerSubAction();
        //if the network, subapp, or networkfiltertype change, switch off service filter
        this.resetSearch();
        //uri gets set after other props are set
        var uriPattern = GetURIPattern(this.name, this.id, this.network, this.node);
        //update the contenturipattern with some of the search params
        this.changeContentURIPattern();
        this.rowParams = this.getRowParams(this.id, this.params, this.clientaction, this.serveraction, this.variable);
    },
    getSearchName: function () {
        var sSearchName = $("#keywords").val();
        sSearchName = RemoveLastAsterisk(sSearchName);
        return sSearchName;
    },
    getOldStartRow: function (index) {
        var startRowId = "#" + SharedUtils.OLDSTART_ROW + index;
        var sStartRow = $(startRowId).val();
        sStartRow = RemoveLastAsterisk(sStartRow);
        return sStartRow;
    },
    getOldParentRow: function (index) {
        var startRowId = "#" + SharedUtils.PARENTSTART_ROW + index;
        var sStartRow = $(startRowId).val();
        sStartRow = RemoveLastAsterisk(sStartRow);
        return sStartRow;
    },
    getOldNetworkId: function () {
        var oldNetworkId = $("#oldNetworkId").val();
        if (oldNetworkId == undefined) {
            oldNetworkId = this.networkId;
        }
        return oldNetworkId;
    },
    getOldServiceId: function () {
        var oldServiceId = $("#oldServiceId").val();
        if (oldServiceId == undefined) {
            oldServiceId = this.serviceId;
        }
        return oldServiceId;
    },
    getOldNetworkType: function () {
        var oldNetworkType = $("#oldNetworkType").val();
        if (oldNetworkType == undefined) {
            oldNetworkType = this.networkFilterType;
        }
        return oldNetworkType;
    },
    getOldNodeName: function () {
        var oldNodeName = $("#oldNodeName").val();
        if (oldNodeName == undefined) {
            oldNodeName = this.currentNodeName;
        }
        return oldNodeName;
    },
    getNetworkFilterType: function () {
        //search which network filter option (i.e. myagreement, mynetworks ...)
        var networkFilterType = $("select#lstNetworkFilter").val();
        if (networkFilterType != undefined && networkFilterType != null) {
            networkFilterType = RemoveLastAsterisk(networkFilterType);
        }
        else {
            networkFilterType = "none";
        }
        return networkFilterType;
    },
    getNetworkId: function () {
        //search which network
        var networkId = $("select#lstNetworks").val();
        if (networkId != undefined && networkId != null) {
            networkId = RemoveLastAsterisk(networkId);
        }
        else {
            networkId = "0";
        }
        return networkId;
    },
    getServiceId: function () {
        //search which network
        var serviceId = $("select#lstRelatedServices").val();
        if (serviceId != undefined && serviceId != null) {
            serviceId = RemoveLastAsterisk(serviceId);
        }
        else {
            serviceId = "0";
        }
        return serviceId;
    },
    getServerSubAction: function () {
        //search alldocs in network or filter by service?
        var subaction = "searchbynetwork";
        if (this.serviceId != 0) {
            subaction = "searchbyservice";
        }
        return subaction;
    },
    resetSearch: function () {
        var oldNetworkFilterType = this.getOldNetworkType();
        var oldNetworkId = this.getOldNetworkId();
        var oldNodeName = this.getOldNodeName();
        var oldServiceId = this.getOldServiceId();
        //switch off service filters when networks switched
        var bNeedsSwitchOff = IsMatch(oldNetworkId, this.networkId) ? false : true;
        if (!bNeedsSwitchOff) bNeedsSwitchOff = IsMatch(oldNodeName, this.currentNodeName) ? false : true;
        if (!bNeedsSwitchOff) bNeedsSwitchOff = IsMatch(oldNetworkFilterType, this.networkFilterType) ? false : true;
        if (bNeedsSwitchOff) {
            //don't switch off when first turning filter on
            bIsNewService = IsMatch(oldServiceId, this.serviceId) ? false : true;
            if (!bIsNewService) {
                this.serviceId = 0;
                this.subaction = "searchbynetwork";
            }
            //reset the start row
            serveraction = isNaN(this.serveraction) ? GetServerAction(this.serveraction) : this.serveraction;
            SetPropertyValue(SharedUtils.START_ROW + serveraction, "value", "0");
        }
    },
    getNodeName: function () {
        //search which nodes from which servicegroup? selection_click changes the select option
        var sSubApp = $("select#lstServiceGroupFilter").val();
        if (sSubApp != undefined) {
            var sNodeName = GetNodeNameFromSubAppType(sSubApp);
            sNodeName = RemoveLastAsterisk(sNodeName);
        }
        else {
            sNodeName = this.node;
        }
        return sNodeName;
    },
    getTypeId: function (currentNodeName) {
        //linkedview type? (filter out calcs and analyses ...)
        var sDocTypeId = null;
        sDocTypeId = $("select#lstSearchTypeFilter").val();
        if (sDocTypeId != null && sDocTypeId != undefined) {
            sDocTypeId = RemoveLastAsterisk(sDocTypeId);
        }
        return sDocTypeId;
    },
    getRowParams: function (id, params, clientaction, serveraction, variable) {
        var sRowParams = "";
        //if this.params contains row params, use those
        var iIndex = (params) ? params.indexOf(SharedUtils.START_ROW) : -1;
        if (iIndex == -1) {
            serveraction = isNaN(serveraction) ? GetServerAction(serveraction) : serveraction;
            var oldStartRow = this.getOldStartRow(serveraction);
            var startRowText = SharedUtils.START_ROW + serveraction;
            var startRow = this.getStartRow(startRowText, oldStartRow);
            var isForward = this.getRowIsForward(variable);
            var parentStartRow = this.getOldParentRow(serveraction);
            var oForm = new Form.FormFilter();
            if (clientaction == SharedUtils.CLIENTSEARCHACTION_search) {
                sRowParams = oForm.getRowParams(oldStartRow, startRow, isForward, this.networkFilterType, parentStartRow);
            }
            else {
                //select searches and content searches don't use a network filter (other than filename)
                sRowParams = oForm.getRowParams(oldStartRow, startRow, isForward, "none", parentStartRow);
            }
        }
        return sRowParams;
    },
    getStartRow: function (startRowText, oldStartRow) {
        var sStartRow;
        var regZero = new RegExp("^0");
        if (regZero.test(startRowText)) {
            //always inits with 0
            sStartRow = startRowText;
        }
        else {
            //use the dom element to find start row
            sStartRow = $("#" + startRowText).val();
        }
        if (sStartRow == undefined && sStartRow == null) sStartRow = oldStartRow;
        sStartRow = RemoveLastAsterisk(sStartRow);
        return sStartRow;
    },
    getRowIsForward: function (variable) {
        var isForward = "-1";
        if (!isNaN(variable)) {
            //pagination buttons use variable to move back and forward
            isForward = variable;
        }
        return isForward;
    },
    changeContentURIPattern: function () {
        var sURIPattern;
        var serveraction = isNaN(this.serveraction) ? GetServerAction(this.serveraction) : this.serveraction;
        if (serveraction == SharedUtils.SERVERACTION_search) {
            if (this.currentNodeName != undefined && this.currentNodeName != null) {
                //page searches don't use uriid, so make that param "0"
                var name = this.searchName;
                var networkid = this.networkId;
                var node = this.currentNodeName;
                if (name == "" || name == undefined || name == null) {
                    name = SharedUtils.NONE;
                }
                if (networkid == undefined || networkid == null) {
                    networkid = 0;
                }
                if (node == undefined || node == null) {
                    node = SharedUtils.NONE;
                }
                this.contenturipattern = MakeContentURIPattern(this.controller,
                    this.serveraction, networkid, node, name,
                    this.id, this.fileextension, this.subaction, this.subactionview,
                    this.variable);
            }
        }
    },
    sendRequest: function (progressId) {
        var sBodyArgs;
        //post back to itself, but send this.contenturipattern in request telling server which uri to process
        var sPostPage = "#";
        var sRequestMethod = "GET";
        var sDisplayElementId;
        var serveraction = isNaN(this.serveraction) ? GetServerAction(this.serveraction) : this.serveraction;
        var eUpdatePanelType = serveraction;
        switch (serveraction) {
            //keep this pattern consistent with onclickpresenter for future use             
            case SharedUtils.SERVERACTION_search:
                sRequestMethod = "GET";
                break;
            default:
                sRequestMethod = "GET";
                break;
        }
        sBodyArgs = this.getBodyArgs(sRequestMethod);
        sDisplayElementId = GetDisplayElementName(eUpdatePanelType, this.subaction);
        this.displayPostRequest(sRequestMethod, eUpdatePanelType, sDisplayElementId, sPostPage, sBodyArgs, progressId);
    },
    getBodyArgs: function (requestmethod) {
        var sBodyArgs;
        var sParams = "";
        if (this.params != undefined && this.params != null) {
            sParams = this.params + this.rowParams;
        }
        else {
            sParams = this.rowParams;
        }
        var oForm = new Form.FormFilter();
        if (this.typeId) {
            //additional category filter used by linkedviews
            sParams += oForm.addFormElement("typeid", this.typeId);
        }
        if (this.searchName) {
            //keyword searches
            sParams += oForm.addFormElement("keywords", this.searchName);
        }
        if (this.serviceId) {
            //service filter
            sParams += oForm.addFormElement("serviceid", this.serviceId);
        }
        sBodyArgs = oForm.buildPostBody(this.contenturipattern, this.clientaction,
            this.subaction, sParams, requestmethod);
        return sBodyArgs;
    },
    displayPostRequest: function (requestMethod, updatePanelType, displayElementId, postPage, bodyArgs, progressId) {
        var onErrorFunction = null;
        //add to view tab click event (could even run asynch after fetching content)
        var onHelpFunction = null;
        //display the appropriate view
        var oChangeClickViewer = new ChangeClickView.ViewChanger(this.contenturipattern, updatePanelType,
            displayElementId, progressId, onErrorFunction, onHelpFunction);
        var sContentType = null;
        oChangeClickViewer.getView(postPage, requestMethod, bodyArgs, sContentType);
    } //displayPostRequest
}