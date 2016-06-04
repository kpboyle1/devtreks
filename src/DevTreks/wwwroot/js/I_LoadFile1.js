
/*
Name:       LoadFile.js
Author:     www.devtreks.org
Date:       2012, September
Purpose:    1. Client file uploading script
            form post targets an inline frame for asynchronous file upload
            called using syntax: var oLoadFile = new LoadFile.FileLoader();
*/


var LoadFile = new Object();
LoadFile.FileLoader = function () {
    this.msgSpan = null;
}
LoadFile.FileLoader.prototype = {
    displayFileUploadClick: function (buttonClickedId, uploadFileParams, spanForMsgId, contenturipattern, selectedFileNameParam) {
        //purpose: no more than one file uploaded at a time 
        var divUploadFile = GetOneElement("divFileUpload");
        if ($(divUploadFile)) {
            //see if straggling fileuploads els around
            var oHasOldFileUploadEls = GetOneElement("divFileUpload1");
            if ($(oHasOldFileUploadEls).length > 0) {
                $(oHasOldFileUploadEls).remove();
            }
            //clone rather than move (in case the moved els are lost when documents are refreshed)
            var divUploadFile1 = $(divUploadFile).clone(true);
            //the live upload els can be identified by a "1" extension to regular file upload el id
            if ($(divUploadFile1).attr("id")) $(divUploadFile1).attr("id", $(divUploadFile1).attr("id") + "1");
            if ($(divUploadFile1).attr("name")) $(divUploadFile1).attr("name", $(divUploadFile1).attr("id"));
            ChangeChildrenIds(divUploadFile1, "1");
            //move the cloned div here
            var oButtonClicked = GetOneElement(buttonClickedId);
            if (oButtonClicked) {
                //append cloned node (mobile uses parent.parent syntax)
                $(oButtonClicked).parent().parent().append($(divUploadFile1));
                //$(oButtonClicked).parent().append($(divUploadFile1));
                var oForm = new Form.FormFilter();
                //append form params needed to be sent back to server on form.submit
                var encontenturipattern = oForm.encodeFormElement(contenturipattern);
                oForm.appendFormInputEl("#frmUploadFile1", SharedUtils.CONTENT_PATTERN, encontenturipattern);
                var enfileUploadParam = oForm.encodeFormElement(uploadFileParams);
                oForm.appendFormInputEl("#frmUploadFile1", SharedUtils.FILE_UPLOAD_PARAMS, enfileUploadParam);
                //display existing upload file message
                var spanUploadFile = GetOneElement(spanForMsgId);
                if ($(spanUploadFile).length > 0) {
                    $(spanUploadFile).show();
                }
            }
        }
    },
    uploadFileClick: function () {
        //for this action, don't allow more than one asynch action at a time
        if (SharedUtils.INTERVAL_ID == 0) {
            var bIsValid = this.verifyUpload();
            if (bIsValid) {
                this.loadFile();
            }
        }
    },
    verifyUpload: function () {
        var bIsValid = false;
        var sFileUploadId = "fileUpload1";
        this.msgSpan = GetOneElement("spanFileUpload1");
        //verify that a file has been selected
        bIsValid = VerifyIsNotEmpty(sFileUploadId, this.msgSpan, "<strong>Please select a file to upload.</strong>");
        if (!bIsValid) return bIsValid;
        //can do other verification if needed here
        return bIsValid;
    },
    verifyXmlFile: function (uploadFileParamsId, elId, msgEl, msg) {
        //true because of binary files
        var bIsOKFile = true;
        var bNeedsXmlFile = false;
        var oUploadFileParams = GetOneElement(uploadFileParamsId);
        if (oUploadFileParams.length > 0) {
            if ($(oUploadFileParams).val().length > 0) {
                //the fileuploadparams will have a datatype of "xml" in their delimited string for xml docs
                var sFileParams = $(oUploadFileParams).val();
                bNeedsXmlFile = IsMatch(";xml;", sFileParams);
                if (bNeedsXmlFile) {
                    bIsOKFile = this.verifyXmlFileExt(elId, msgEl, msg)
                }
            }
        }
        return bIsOKFile;
    }, //verifyXmlFile
    verifyXmlFileExt: function (elId, msgEl, msg) {
        var bIsXml = false;
        var oUploadFileEl = GetOneElement(elId);
        if (oUploadFileEl.length > 0) {
            if ($(oUploadFileEl).val().length > 0) {
                var sFilePath = $(oUploadFileEl).val();
                //acceptable file extensions in xml data type path (assumes html files are actually xhtml-compliant)
                var regXmlExt = /\.(xml|xslt|xsl|xhtml|html)$/i;
                //true if the file being uploaded has an acceptable file extension
                bIsXml = IsMatch(regXmlExt, sFilePath);
                //certainly possible to test xml on client as well
                //ChangeClickView.setDevPack()
                if (!bIsXml && msgEl) {
                    $(msgEl).html(msg);
                    $(oUploadFileEl).focus();
                }
            }
        }
        return bIsXml;
    }, //verifyXmlFileExt
    loadFile: function () {
        //start file upload progress feedback when ajax response can be handled
        this.startFeedback();
        var $formUpload = $("#frmUploadFile1")[0];
        if ($formUpload.length > 0) {
            $formUpload.target = "frameFileUpload";
            //submit the form
            $formUpload.submit();
        }
    }, //loadFile
    startFeedback: function () {
        //this starts the .... animation
        if (SharedUtils.INTERVAL_ID == null || SharedUtils.INTERVAL_ID == 0) {
            var buttonSubmit = $("#btnFileUpload1");
            if ($(buttonSubmit).length > 0) {
                $(buttonSubmit).hide();
            }
            var iIntervalId = window.setInterval("DisplayAsynchFeedback('" + "spanFileUpload1" + "', '<strong>Uploading</strong>" + "')", 250);
            SharedUtils.INTERVAL_ID = parseInt(iIntervalId);
        }
    },
    uploadFinish: function () {
        //this fires from   $("#frameFileUpload").bind("load", uploadFinish);
        if (SharedUtils.INTERVAL_ID != 0) {
            var oSpan = $("#spanFileUpload1");
            if ($(oSpan).length > 0) {
                //if the span has children, its giving feedback about a pending file upload
                if ($(oSpan)) {
                    ClearInterval("spanFileUpload1");
                    var hasServerError = HasServerErrorMessage();
                    var needsRemoval = true;
                    var contenturipattern = GetFormElementValue("frmUploadFile1", SharedUtils.CONTENT_PATTERN, needsRemoval);
                    if (contenturipattern) {
                        var newcontenturipattern = ChangeContentURIPatternPart(contenturipattern, SharedUtils.URIPATTERNPART_subaction, "respondwithhtml");
                        this.startUploadStatusFeedback();
                        var fileUploadParams1 = GetFormElementValue("frmUploadFile1", SharedUtils.FILE_UPLOAD_PARAMS, needsRemoval);
                        var bIsMetaData = IsMatch("MetaData", GetSubStringWithDelimiter(fileUploadParams1, ";", 1));
                        if (bIsMetaData) {
                            //to show the meta doc - probably should display in views panel
                            //pause, asynch postback and respond with the newly added resource (i.e. show the image or converted xmldocument)
                            devTreksOnClick(newcontenturipattern, SharedUtils.CLIENTACTION_postrequest, "");
                        }
                        else {
                            //pause, asynch postback and respond with the newly added resource (i.e. show the image or converted xmldocument)
                            devTreksOnClick(newcontenturipattern, SharedUtils.CLIENTACTION_postrequest, "");
                            //window.setTimeout("devTreksOnClick(" + contenturipattern + "," + SharedUtils.CLIENTACTION_postrequest + "," + '' + ")", 3000);
                        }
                    }
                    else {
                        oSpan.html("The site had trouble verifying the status of the upload. Please refresh this page to verify the status of the upload.");
                    }
                }
            }
        }
    },
    startUploadStatusFeedback: function () {
        var iIntervalId = window.setInterval("DisplayAsynchFeedback('" + "spanFileUpload1" + "', '<strong>Refreshing page</strong>" + "')", 250);
        SharedUtils.INTERVAL_ID = parseInt(iIntervalId);
    },
    removeFileUploadEls: function () {
        var divUploadFile = GetOneElement("divFileUpload1");
        if (divUploadFile) {
            $(divUploadFile).remove();
        }
    }, //removeFileUploadEls
    getFileUploadParams: function () {
        //get the upload file params from the hidden input element
        var sParams = null;
        var inputUploadFile = GetOneElement("fileUploadParams");
        if (inputUploadFile) {
            sParams = $(inputUploadFile).val();
        }
        return sParams;
    }, //getFileUploadParams
    getFilePath: function (elId) {
        var sFilePath;
        var oUploadFileEl = GetOneElement(fileUploadElId);
        if (oUploadFileEl) {
            //server will only process this file path (only one file at a time can be saved)
            sFilePath = $(oUploadFileEl).val();
        }
        return sFilePath;
    }, //getFilePath
    makeFileUploadOnClickArg: function () {
        var sArgument = "javascript:uploadFileClick('" + this.fileName + "', " + this.serveraction + ", '" + this.attName + "');return false;";
        return sArgument;
    },
    handleServerResponse: function (request) {
        this.runScript(request.responseText);
    },
    runScript: function (script) {
        //calls either setDevPack or setXsltDoc
        eval(script);
    },
    defaultError: function () {
        alert("couldn't upload file");
    }
}