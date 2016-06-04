/*
    Name:ChangeClickView.js
    Author: www.devtreks.org
    Date: 2012, September
    Purpose: respond to doc click events 
    //called using syntax: var oChangeClickViewer = new ChangeClickView.ViewChanger(params);
*/
//constructor
var ChangeClickView = new Object();
ChangeClickView.ViewChanger=function(){}
ChangeClickView.ViewChanger=function(contenturipattern, updatePanelType, 
    displayElementId, progressId, onError, onHelp){
    this.onHelp  = (onHelp) ? onHelp : this.defaultHelp;
    this.onError = (onError) ? onError : this.defaultError;
    this.contenturipattern = contenturipattern;
    this.updatePanelType = updatePanelType;
    this.xmlDoc = null;
    this.params = null;
    this.xsltDoc = null;
    this.displayElementId = displayElementId;
    this.progressId = progressId;
}
//object literal
ChangeClickView.ViewChanger.prototype = {
    getView: function (postPage, requestMethod, params, contentType) {
        if (this.displayElementId) {
            try {
                //start asynch feedback
                this.startAsynchFeedback(this.progressId);
                if (this.updatePanelType == SharedUtils.UPDATE_PANEL_TYPES_pack) {
                    //see if the browser supports xslt transforms
                    if (IsMatch("divDownloadPackLink", this.displayElementId)) {
                        HideOrShowPackageCommands(false);
                        //jQuery handles displaying server response
                        this.displayData(SharedUtils.RESPONSETYPE_html, postPage, requestMethod,
                            params, contentType);
                    }
                    else {
                        //jQuery handles displaying server response
                        //put the server response in a devpack and transform
                        //get the xml data
                        this.displayData(SharedUtils.RESPONSETYPE_xml, postPage,
                            requestMethod, params, contentType);
                        //put the server response in an xsltdoc and transform
                        //get the xslt stylesheet
                        alert("An error message is about to occur. The feature that transforms xml into html on the client has not been refactored for jQuery and Ajax yet.");
                        var sPostPage = "StylesheetController.cshtml";
                        this.displayData(SharedUtils.RESPONSETYPE_xslt, sPostPage, requestMethod,
                            params, contentType);
                    }
                }
                else {
                    //jQuery handles displaying server response
                    this.displayData(SharedUtils.RESPONSETYPE_html, postPage, requestMethod,
                        params, contentType);
                }
            }
            catch (e) {
                var sErrorNotice = "error:" + e.description;
                //display error message
                AddMessageHtml(sErrorNotice, true);
            }
        }
    },
    startAsynchFeedback: function (progressId) {
        //change tabs
        if (this.displayElementId != "divsearch") {
            this.changeTabs(this.displayElementId);
        }
        //fade out of previous content in panel
        FadeOut(this.displayElementId);
        //progress animation
        if (SharedUtils.INTERVAL_ID == null || SharedUtils.INTERVAL_ID == 0) {
            var iIntervalId;
            if (progressId) {
                iIntervalId = window.setInterval("DisplayAsynchFeedback('" + progressId + "', '')", 250);
            }
            else {
                iIntervalId = window.setInterval("DisplayAsynchFeedback('" + "spanGettingData" + "', '')", 250);
            }
            SharedUtils.INTERVAL_ID = parseInt(iIntervalId);
        }
    },
    displayNetData: function (updatePanelType, postPage, onError,
        requestMethod, params, contentType, displayElementId) {
        //load the data and display using .net web request handler
        var oLoader = new LoadNetData.NetContentLoader(postPage,
            this.onError, this.onHelpFunction, requestMethod, params, contentType,
            updatePanelType, displayElementId);
    },
    displayData: function (responseType, postPage, requestMethod, params, contentType) {
        //make an ajax request
        $.ajax({
            url: postPage,
            type: requestMethod,
            dataType: GetResponseType(responseType),
            timeout: 1000000, //refactor: test server change for production
            cache: false, //don't cache in browser: data is dynamic and changes frequently
            data: params, //params
            error: function (data, textStatus, errorThrown) {
                //this no longer refers to the ChangeClickView object but the request
                var oChangeClickViewer = new ChangeClickView.ViewChanger("",
                    null, null, null, null, null);
                //set oChangeClickViewer properties using the request params sent in
                oChangeClickViewer.setDisplayParams(params);
                oChangeClickViewer.displayError(this, data, errorThrown)
            },
            success: function (data, textStatus) {
                var oChangeClickViewer = new ChangeClickView.ViewChanger("",
                    null, null, null, null, null);
                oChangeClickViewer.setDisplayParams(params);
                oChangeClickViewer.displayResponse(this, data, textStatus)
            }
        });
    },
    setDisplayParams: function (requestParams) {
        if (requestParams) {
            try {
                var oForm = new Form.FormFilter();
                this.contenturipattern = oForm.getFormElementValue(SharedUtils.CONTENT_PATTERN, requestParams);
                var clientaction = oForm.getFormElementValue(SharedUtils.CLIENT_ACTION, requestParams);
                var serveraction = GetContentURIPatternPart(this.contenturipattern, SharedUtils.URIPATTERNPART_action);
                serveraction = isNaN(serveraction) ? GetServerAction(serveraction) : serveraction;
                clientaction = isNaN(clientaction) ? GetClientAction(clientaction) : clientaction;
                var subaction = GetContentURIPatternPart(this.contenturipattern, SharedUtils.URIPATTERNPART_subaction);
                subaction = isNaN(subaction) ? GetServerSubAction(subaction) : subaction;
                this.updatePanelType = serveraction;
                this.displayElementId = GetDisplayElementName(this.updatePanelType, subaction);
                this.progressId = GetProgressId(clientaction);
            }
            catch (e) {
                var sErrorNotice = "error:" + e.description;
                //display error message
                AddMessageHtml(sErrorNotice, true);
            }
        }
    },
    displayResponse: function (xhr, data, textStatus) {
        if (data) {
            if (xhr.dataType == "xml") {
                this.setXml(data);
            }
            else if (xhr.dataType == "xslt") {
                this.params = xhr.data;
                this.setXsltDoc(data);
            }
            else if (xhr.dataType == "html"
                || xhr.dataType == "text") {
                this.displayHtml(data);
            }
        }
    },
    setXml: function (data) {
        this.xmlDoc = data;
        if (this.xmlDoc.parseError.errorCode != 0) {
            var xmlError = this.xmlDoc.parseError;
            alert(xmlError);
        }
        else {
            this.displayXmlUsingXslt();
        }
    },
    setXsltDoc: function (data) {
        //this is a docdocument, not a free-threaded dom so need later transform
        this.xsltDoc = data;
        if (this.xsltDoc.parseError.errorCode != 0) {
            var xsltError = this.xsltDoc.parseError;
            alert(xsltError);
        }
        else {
            this.displayXmlUsingXslt();
        }
    },
    displayXmlUsingXslt: function () {
        //can't be sure the order this function will be called (last call means both on hand)
        if (this.xmlDoc == null || this.xsltDoc == null) { return; }
        var sEditButtonHtml = this.getEditDocButtons();
        if (window.ActiveXObject) {
            this.transformIEDoc(sEditButtonHtml);
        }
        else if (window.XmlHttpRequest && window.XSLTProcessor) {
            this.transformMozillaDoc(sEditButtonHtml);
        }
        //now change the tabs
        this.changeTabs(this.displayElementId);
    },
    transformIEDoc: function (editButtonHtml) {
        var xslt = new ActiveXObject("Msxml2.XSLTemplate.3.0");
        var xsltFTDoc = new ActiveXObject("Msxml2.FreeThreadedDOMDocument.3.0");
        var xsltProcessor;
        xsltFTDoc.async = false;
        //this.xsltDoc is a domdocument that is not free-threaded, so must be converted to a string and then used
        xsltFTDoc.loadXML(this.xsltDoc.xml.toString());
        xslt.stylesheet = xsltFTDoc;
        xsltProcessor = xslt.createProcessor();
        xsltProcessor.input = this.xmlDoc;
        //add the params (needs to match serverside standardard parameter list)
        this.addXsltParameters(xsltProcessor);
        xsltProcessor.transform();
        var oDisplayElement = GetOneElement(this.displayElementId);
        if (oDisplayElement) {
            oDisplayElement.innerHTML = editButtonHtml + xsltProcessor.output;
        }
    }, //transformIEDoc
    transformMozillaDoc: function (editButtonHtml) {
        var xsltProcessor = new XSLTProcessor();
        xsltProcessor.importStylesheet(this.xsltDoc);
        //add the params
        this.addXsltParameters(xsltProcessor);
        var oHTML = xsltProcessor.transformToFragment(this.xmlDoc, document);
        var oDisplayElement = GetOneElement(this.displayElementId);
        if (oDisplayElement) {
            oDisplayElement.textContent = "";
            oDisplayElement.appendChild(editButtonHtml);
            oDisplayElement.appendChild(oHTML);
        }
    }, //transformMozillaDoc
    displayHtml: function (data) {
        //finish asynch feedback
        ClearInterval(this.progressId, this.displayElementId);
        if (IsMatch("divDownloadPackLink", this.displayElementId)) {
            HideOrShowPackageCommands(true);
        }
        if (data) {
            var oDisplayElement = GetOneElement(this.displayElementId);
            if (oDisplayElement != null) {
                oDisplayElement.html("");
                //tells jquery to using mobile styling on ajax html inserts
                //$( ...new markup that contains widgets... ).appendTo( ".ui-page" ).trigger( "create" ); 
                $($.parseHTML(data)).appendTo("#" + this.displayElementId).enhanceWithin();
                //$.parseHTML(data).appendTo("#" + this.displayElementId).trigger("create");
                oDisplayElement.show();
            }
            //second and third menus have finished loading and can now be initialized
            var sTabId = null;
            var oChangeTabViewer = new ChangeTabView.ViewChanger(null, sTabId, this.updatePanelType);
            //see if images (...) need to be cached
        }
    }, //displayHtml
    addXsltParameters: function (xsltProcessor) {
        //standard stylesheet params sent by server as an onclick argument
        if (this.params) {
            var arrParams = this.params.split(";");
            if (arrParams) {
                var arrParam;
                var sName;
                var sValue;
                for (var i = 0; i < arrParams.length; i++) {
                    arrParam = arrParams[i].split("=");
                    if (arrParam) {
                        sName = arrParam[0];
                        sValue = arrParam[1];
                        if (!sName || !sValue) continue;
                        if (xsltProcessor.setParameter) {
                            //Mozilla
                            xsltProcessor.setParameter(null, sName, sValue);
                        }
                        else {
                            //IE
                            xsltProcessor.addParameter(sName, sValue);
                        }
                    }
                }
            }
        }

    },
    getParams: function () {
        alert(this.params);
        //not any good; send the std ss params in the onclick event
        return null;
    },
    getEditDocButtons: function () {
        var sEditButtonHtml;
        var sSelectParams = "";
        var sEditCommand = GetDevPackClickArgument(this.contenturipattern, SharedUtils.CLIENTACTION_postrequest,
            SharedUtils.SERVERACTION_pack, SharedUtils.SERVERSUBACTION_submitedits);
        sEditButtonHtml = MakeInputString("button", "SubmitButton1Enabled150", "submitedits", sEditCommand, "Edit", "");
        return sEditButtonHtml;
    },
    changeTabs: function (displayElementId) {
        //change the displayelementid (i.e divsearch) to tab.id (i.e. search)
        var tabId = ReplaceString(displayElementId, "div", "");
        //change the tab
        var oChangeTabViewer = new ChangeTabView.ViewChanger(null, tabId);
    },
    defaultHelp: function () {
        alert("no help available for this action");
    },
    defaultError: function () {
        //finish asynch feedback
        ClearInterval(this.progressId, this.displayElementId);
        alert("No error message is available for this action. ChangeClickView.defaultError raised.");
    },
    displayError: function (xhr, data, errorThrown) {
        //finish asynch feedback
        ClearInterval(this.progressId, this.displayElementId);
        var sHelpNotice;
        if (data.readyState == 4) {
            sHelpNotice = "An error occurred trying to fetch the information you requested. Please try again.";
            sHelpNotice += "<ul><li>error:" + data.responseText + "</li>";
            sHelpNotice += "<li>readystate:" + data.readyState + "</li>";
            sHelpNotice += "<li>status:" + data.status + "</li>";
            sHelpNotice += "<li>headers:" + data.readyState + "</li>";
            if (data.getAllResponseHeaders()) {
                sHelpNotice += "<li>readystate:" + data.getAllResponseHeaders() + "</li>";
            }
            sHelpNotice += "</ul>"
        }
        else {
            sHelpNotice = "The server timed out. The action you are taking is too big for this server to handle at this time.";
        }
        //display error message
        this.displayHtml(sHelpNotice);
        //AddMessageHtml(sHelpNotice, true);
    }
}