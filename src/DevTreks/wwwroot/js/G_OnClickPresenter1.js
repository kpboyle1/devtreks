/*
    Name:OnClickPresenter.js
    Author: www.devtreks.org
    Date: 2011, July
    Purpose: handler for devtrek click events and methods
    called using syntax: var ChangeClient = new ChangeClient.ClientChanger('url.xml, MyCallBackFunction);
*/

//devtreks standard onclick method
function devTreksOnClick(contenturipattern, clientaction, params){
    //usually involves a calculator or an xml document edit, where params are important
    //routing patterns are in onclick method
    var presentationChanger = new OnAjaxClickPresenter.ChangePresentation(
        contenturipattern, clientaction, params);
    if (presentationChanger.serveraction == SharedUtils.SERVERACTION_search 
        || presentationChanger.serveraction == "search"){
        var searchChanger = new OnSearchAjaxClickPresenter.ChangePresentation(presentationChanger);
        searchChanger.onClick();
    }
    else {
        presentationChanger.onClick();
    }
    
}
//object constructor
var OnAjaxClickPresenter = new Object();
OnAjaxClickPresenter.ChangePresentation = function (contenturipattern, clientaction, params) {
    if (contenturipattern != null && contenturipattern != undefined && contenturipattern != "") {
        this.contenturipattern = contenturipattern;
        this.controller = GetContentURIPatternPart(this.contenturipattern, SharedUtils.URIPATTERNPART_controller);
        this.serveraction = GetContentURIPatternPart(this.contenturipattern, SharedUtils.URIPATTERNPART_action);
        this.network = GetContentURIPatternPart(this.contenturipattern, SharedUtils.URIPATTERNPART_network);
        this.node = GetContentURIPatternPart(this.contenturipattern, SharedUtils.URIPATTERNPART_node);
        this.name = GetContentURIPatternPart(this.contenturipattern, SharedUtils.URIPATTERNPART_name);
        this.id = GetContentURIPatternPart(this.contenturipattern, SharedUtils.URIPATTERNPART_id);
        this.fileextension = GetContentURIPatternPart(this.contenturipattern, SharedUtils.URIPATTERNPART_fileextension);
        this.subaction = GetContentURIPatternPart(this.contenturipattern, SharedUtils.URIPATTERNPART_subaction);
        this.subactionview = GetContentURIPatternPart(this.contenturipattern, SharedUtils.URIPATTERNPART_subactionview);
        this.variable = GetContentURIPatternPart(this.contenturipattern, SharedUtils.URIPATTERNPART_variable);
        this.className = "";
        this.uriPattern = GetURIPattern(this.name, this.id, this.network, this.node);
        if (clientaction != null && clientaction != undefined) {
            this.clientaction = clientaction;
        }
        else {
            this.clientaction = SharedUtils.CLIENTACTION_postrequest;
        }
        //not included in route patterns
        this.clientaction = isNaN(this.clientaction) ? GetClientAction(this.clientaction) : this.clientaction;
        if (params != null && params != undefined) {
            this.params = TrimStylesheetParams(params);
        }
        else {
            this.params = "";
        }
        if (!isNaN(this.params)) {
            //interferes with string manipulations
            this.params = "";
        }
    }
}
//encapsulate object props, methods, events in an object literal
OnAjaxClickPresenter.ChangePresentation.prototype = {
    onClick: function () {
        //init this.object specific properties
        this.initParams();
        var sProgressId = GetProgressId(this.clientaction);
        switch (this.clientaction) {
            case SharedUtils.CLIENTACTION_postrequest:
                //always look for a selectedLinkedView in this.params.selectedlinkedviewboxid select box
                this.addSelectedLinkedViewToFormEls();
                this.sendRequest(sProgressId);
                break;
            case SharedUtils.CLIENTACTION_closeelement:
                CloseDiv(this.serveraction, this.subaction);
                return;
                break;
            case SharedUtils.CLIENTACTION_changeurl:
                if (this.subaction == "gohome") {
                    window.location = "Default.cshtml";
                }
                else {
                    this.changeLocation();
                }
                break;
            case SharedUtils.CLIENTACTION_openwindow:
                window.open(this.uriPattern);
                break;
            case SharedUtils.CLIENTACTION_loaddoc:
                //straight xml get request method?
                break;
            case SharedUtils.CLIENTACTION_addtopack:
                //add current file to package (for download, further action)
                //this does not make a request to server - uses client-side javascript
                this.addToPackList();
                break;
            case SharedUtils.CLIENTACTION_downloaddoc:
                //put std zip file args[] into params
                var hasGoodZipParams = this.setZipParams();
                if (hasGoodZipParams) {
                    if (this.subaction == "makepackage") {
                        //put package args into params
                        this.setPackageParams();
                    }
                    //standard asynch post request (i.e. giving progress)
                    this.sendRequest(sProgressId);
                }
                else {
                    AddMessageHtml("Please first select files to package", true);
                }
                break;
            case SharedUtils.CLIENTACTION_prepaddin:
                //always look for a selectedLinkedView in this.params.selectedlinkedviewboxid select box
                this.addSelectedLinkedViewToFormEls();
                //the server subaction tells oForm.buildPostBody to include all form elements in the postback
                this.sendRequest(sProgressId);
                this.reSetAddInStep();
                break;
            default:
                //do nothing
                break;
        }
    }, //onclick
    initParams: function () {
        this.rowParams = this.getRowParams(this.id, this.clientaction, this.serveraction, this.variable);
        if (this.className == "AjaxLink") {
            this.clientaction = SharedUtils.CLIENTACTION_postrequest;
            HighlightElementClicked(this.uriPattern);
        }
        else if (this.className == "JSLink") {
            this.clientaction = SharedUtils.CLIENTACTION_postrequest;
        }
        else {
            //go with those already set
        }
    },
    sendRequest: function (progressId) {
        var sBodyArgs;
        var sParams = this.addRowParams();
        //post back to itself, but send this.contenturipattern in request telling server which uri to process
        var sPostPage = "#";
        var sRequestMethod = "GET";
        var sDisplayElementId;
        var serveraction = isNaN(this.serveraction) ? GetServerAction(this.serveraction) : this.serveraction;
        var subaction = isNaN(this.subaction) ? GetServerSubAction(this.subaction) : this.subaction;
        var eUpdatePanelType = serveraction;
        //check for selections pending
        var needsPost = false;
        var selections = GetSelections(serveraction);
        if (selections.length > 1) {
            sParams += selections;
            needsPost = true;
        }
        if (subaction == SharedUtils.SERVERSUBACTION_submitedits
            || subaction == SharedUtils.SERVERSUBACTION_uploadfile
            || subaction == SharedUtils.SERVERSUBACTION_submitlistedits
            || subaction == SharedUtils.SERVERSUBACTION_submitformedits
            || subaction == SharedUtils.SERVERSUBACTION_adddefaults
            || subaction == SharedUtils.SERVERSUBACTION_saveselects
            || subaction == SharedUtils.SERVERSUBACTION_runaddin
            || subaction == SharedUtils.SERVERSUBACTION_makepackage
            || subaction == SharedUtils.SERVERSUBACTION_buildtempdoc
            || needsPost) {
            sRequestMethod = "POST";
            var controller = GetContentURIPatternPart(this.contenturipattern, SharedUtils.URIPATTERNPART_controller);
            //jquery posts to an absolute path which asp.net won't recognize
            sPostPage = GetPostRelativePath() + controller;
        }
        var oForm = new Form.FormFilter();
        sBodyArgs = oForm.buildPostBody(this.contenturipattern, this.clientaction,
            this.subaction, sParams, sRequestMethod);
        sDisplayElementId = GetDisplayElementName(eUpdatePanelType, this.subaction);
        this.displayPostRequest(sRequestMethod, eUpdatePanelType, sDisplayElementId, sPostPage, sBodyArgs, progressId);
    }, //sendRequest
    displayPostRequest: function (requestMethod, updatePanelType, displayElementId, postPage, bodyArgs, progressId) {
        var onErrorFunction = null;
        //add to view tab click event (could even run asynch after fetching content)
        var onHelpFunction = null;
        //display the appropriate view
        var oChangeClickViewer = new ChangeClickView.ViewChanger(this.contenturipattern, updatePanelType,
            displayElementId, progressId, onErrorFunction, onHelpFunction);
        var sContentType = null;
        oChangeClickViewer.getView(postPage, requestMethod, bodyArgs, sContentType);
    }, //displayPostRequest
    getRowParams: function (id, clientaction, serveraction, variable) {
        var sRowParams;
        var oPresentationChanger = new OnSearchAjaxClickPresenter.ChangePresentation();
        sRowParams = oPresentationChanger.getRowParams(id, this.params, clientaction, serveraction, variable);
        return sRowParams;
    },
    addRowParams: function () {
        var sParams = this.params + this.rowParams;
        return sParams;
    },
    changeLocation: function () {
        var oForm = new Form.FormFilter();
        var sBodyArgs = oForm.buildPostBody(this.contenturipattern,
            this.clientaction, this.subaction, "GET");
        window.location = sBodyArgs;
    }, //changeLocation
    reSetAddInStep: function () {
        var bIsMatch = IsMatch("&step=stepzero", this.params);
        if (bIsMatch) {
            SharedUtils.TabId = "stepzero";
        }
    },
    addToPackList: function () {
        //add the current document to a package list (for zip download or making into a package)
        $("#divpack").hide();
        $("#divFileList").show();
        var oChangeTabViewer = new ChangeTabView.ViewChanger(this.defaultError, "",
            SharedUtils.UPDATE_PANEL_TYPES_pack);
        AddFileToList("olFileList", this.contenturipattern,
            SharedUtils.URIBASE, this.params);
    },
    setZipParams: function () {
        var hasGoodZipParams = false;
        var oForm = new Form.FormFilter();
        var sFileType = GetRadioValue("rdoFileList");
        //distinguish html and xml downloads
        this.params += oForm.addFormElement("filetype", sFileType);
        //make a form el, zipparams="std arg array expected by server zip utility"
        var sArgsDelimiter = ";";
        var sArgs = this.contenturipattern;
        //var sPassword = document.getElementById (signin.pwd;
        // sArgs += + sArgsDelimiter + "-p" + sArgsDelimiter + sPassword;
        this.params += oForm.addFormElement("zipargs", sArgs);
        //these uripatterns include a "*" last char that server will delete
        var sFiles = AddChildFirstChildIds("olFileList", sArgsDelimiter);
        if (sFiles) {
            if (sFiles.length > 0) {
                this.params += oForm.addFormElement("zipfiles", sFiles);
                //the last parameter of the onclick event is a docpath to the file
                var sOnClicks = AddChildSecondChildOnClickParams("olFileList", sArgsDelimiter);
                this.params += oForm.addFormElement("zipdocpaths", sOnClicks);
                hasGoodZipParams = true;
            }
        }
        oForm = null;
        return hasGoodZipParams;
    },
    setPackageParams: function () {
        var oForm = new Form.FormFilter();
        var sRelatedDataType = GetRadioValue("rdoRelatedDataType");
        //make a form el for sibling and children data
        this.params += oForm.addFormElement("relateddatatype", sRelatedDataType);
        var sPackageType = GetRadioValue("rdoPackageType");
        //make a form el to distinguish the type of package to make
        this.params += oForm.addFormElement("packagetype", sPackageType);
        //digital signature type
        var sNeedsDigitalSignature = GetRadioValue("rdoSignatureType");
        //yes = sign all parts; no = don't sign parts
        this.params += oForm.addFormElement("digitalsignaturetype", sNeedsDigitalSignature);
        oForm = null;
    },
    addSelectedLinkedViewToFormEls: function () {
        //see if a linked view is being selected by a Get [View] click event
        var regLinkedViewListId = new RegExp("selectslinkedviewsid");
        if (regLinkedViewListId.test(this.params)) {
            //convention is to put the name of the select linked view box name 
            //in last position
            sSelectLinkedViewsBoxId = GetLastSubstring(this.params, SharedUtils.EqualDelimiter);
            if (sSelectLinkedViewsBoxId) {
                this.addSelectedValueToParams(sSelectLinkedViewsBoxId);
                if (sSelectLinkedViewsBoxId == "lstAddinLinkedViews") {
                    var sNodeName = GetSubStringWithDelimiter(this.uriPattern, SharedUtils.URIPatternDelimiter, 1);
                    if (sNodeName != null && sNodeName != undefined) {
                        var regNodeName = new RegExp("linkedview");
                        if (regNodeName.test(sNodeName)) {
                            //have the addin uri, but also need the linkedview (for custom doc calcs)
                            this.addSelectedValueToParams("lstViewLinkedViews");
                        }
                        else {
                            regNodeName = new RegExp("devpack");
                            if (regNodeName.test(sNodeName)) {
                                //have the addin uri, but also need the linkedview (for custom doc calcs)
                                this.addSelectedValueToParams("lstViewLinkedViews");
                            }
                        }
                    }
                }
            }
        }
    },
    addSelectedValueToParams: function (selectBoxId) {
        var sSelectedLinkedViewURIPattern = GetSelectValue(null, selectBoxId);
        if (sSelectedLinkedViewURIPattern != null) {
            var oForm = new Form.FormFilter();
            this.params += oForm.addFormElement(selectBoxId, sSelectedLinkedViewURIPattern);
            oForm = null;
        }
    },
    defaultError: function () {
        alert("couldn't handle onclick in onclickpresenter");
    }
}