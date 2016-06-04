
/*
Name:       Form.js
Author:	    www.devtreks.org
Date:       2015, April
Purpose:    filter form element submittals. 
Notes       1. A Get http method puts the params in the url, 
            a Post puts them in form collection.
            2. contenturipattern is the main param used for navigation
            
*/
var Form = new Object();
Form.FormFilter = function () {
}
Form.FormFilter.prototype = {
    buildPostBody: function (contenturipattern, clientaction, subaction, params, requestmethod) {
        var serveraction = GetContentURIPatternPart(contenturipattern, SharedUtils.URIPATTERNPART_action);
        serveraction = isNaN(serveraction) ? GetServerAction(serveraction) : serveraction;
        subaction = isNaN(subaction) ? GetServerSubAction(subaction) : subaction;
        contenturipattern = this.getCUPFromForm(serveraction, subaction, contenturipattern);
        contenturipattern = this.encodeFormElement(contenturipattern);
        var sBodyArgs = SharedUtils.CONTENT_PATTERN + SharedUtils.EqualDelimiter + contenturipattern;
        //enums (note: not needed by server, but needed in ChangeClickView.setDisplayParams())
        sBodyArgs += this.addFormElement(SharedUtils.CLIENT_ACTION, clientaction);
        //misc. params used on server by serversubaction 
        sBodyArgs += (params != null && params != undefined) ? params : "";
        if (requestmethod == "POST") {
            //form posts
            sBodyArgs += this.addFormElements(contenturipattern, params, serveraction, subaction);
            //form elements can be found on other panels as of 0.8.0
            sBodyArgs += this.addOtherElements(params, serveraction, subaction);
        }
        return sBodyArgs;
    },
    getCUPFromForm: function (serveraction, subaction, contenturipattern) {
        var sContentURIPattern = contenturipattern;
        if (serveraction == SharedUtils.SERVERACTION_edit) {
            if (subaction == SharedUtils.SERVERSUBACTION_saveselects) {
                //existing contenturipattern is the file where selections are being made
                //need to put them here
                var oFileSelects = GetOneElement("selectsuripattern");
                if ($(oFileSelects).length > 0) {
                    sContentURIPattern = $(oFileSelects).val();
                }
                else {
                    var fileExt = GetContentURIPatternPart(sContentURIPattern, SharedUtils.URIPATTERNPART_fileextension);
                    if (fileExt != "temp") {
                        //version 1 doesn't allow temp docs
                        //building a new temp doc (inits with id=0 and new subactionview)
                        //sContentURIPattern = ChangeContentURIPatternPart(sContentURIPattern, SharedUtils.URIPATTERNPART_id, "0");
                        //sContentURIPattern = ChangeContentURIPatternPart(sContentURIPattern, SharedUtils.URIPATTERNPART_subactionview, "buildnewdocview");
                    }
                }
            }
        }
        return sContentURIPattern;
    },
    addFormElements: function (contenturipattern, params, serveraction, subaction) {
        var jqEditFormArray = this.getFormArray(contenturipattern, serveraction, subaction);
        var sFormElements = "";
        if (jqEditFormArray) {
            var i; var sName; var sValue; var sFormElement; var iIndex;
            var oForm = this;
            jQuery.each(jqEditFormArray, function (i) {
                iIndex = -1;
                sName = this.name;
                if (!sName) {
                    sName = this.id;
                }
                sValue = this.value;
                sFormElement = "";
                bIsMatch = false;
                //add to filtered form collection
                sFormElement = oForm.addEditFormElement(this, subaction, params, sName, sValue);
                //make sure it has not already been added
                if (sFormElement != "") {
                    iIndex = params.indexOf(sFormElement);
                    if (iIndex == -1) {
                        sFormElements += sFormElement;
                    }
                }
            });
        }
        return sFormElements;
    },
    getFormArray: function (contenturipattern, serveraction, subaction) {
        var jqEditFormArray = "";
        if (serveraction == SharedUtils.SERVERACTION_edit) {
            if (subaction == SharedUtils.SERVERSUBACTION_saveselects) {
                jqEditFormArray = $("#frmSelect")[0];
            }
            else {
                jqEditFormArray = $("#frmEdit")[0];
            }
        }
        else if (serveraction == SharedUtils.SERVERACTION_linkedviews) {
            jqEditFormArray = $("#frmLinkedViews")[0];
        }
        else if (serveraction == SharedUtils.SERVERACTION_pack) {
            jqEditFormArray = $("#frmPack")[0];
        }
        return jqEditFormArray;
    },
    addFormElement: function (name, value) {
        var sFormElement = SharedUtils.AndDelimiter + name + SharedUtils.EqualDelimiter + value;
        return sFormElement;
    },
    getFormElementValue: function (name, stdFormEls) {
        var sFormElementValue = "";
        var iStart = stdFormEls.lastIndexOf(name + SharedUtils.EqualDelimiter);
        var iStartNameLength = name.length + 1;
        var iEnd = stdFormEls.indexOf(SharedUtils.AndDelimiter, iStart + iStartNameLength);
        sFormElementValue = stdFormEls.substring(parseInt(iStart + iStartNameLength), parseInt(iEnd));
        return sFormElementValue;
    },
    getLastFormElementURIPattern: function (stdFormEls) {
        var sURIPattern = "";
        var sFormElementValue = GetLastSubstring(stdFormEls, SharedUtils.AndDelimiter);
        if (sFormElementValue) {
            //the record being edited is the first param in edit value
            sURIPattern = GetSubStringWithDelimiter(sFormElementValue, ";", 0);
        }
        return sURIPattern;
    },
    validateForm: function (name, value) {
        var sFormElement = SharedUtils.AndDelimiter + name + SharedUtils.EqualDelimiter + value;
        return sFormElement;
    },
    addEditFormElement: function (formElement, subaction, params, name, value) {
        //filter form elements
        var sFormElement = "";
        var bOKToAdd = false;
        switch (subaction) {
            case SharedUtils.SERVERSUBACTION_submitedits:
                bOKToAdd = this.lastCharIsMatch(value);
                if (this.isDeletion(value)) {
                    bOKToAdd = this.confirmDeletion(formElement, name, value);
                }
                break;
            case SharedUtils.SERVERSUBACTION_submitlistedits:
                bOKToAdd = this.lastCharIsMatch(value);
                if (this.isDeletion(value)) {
                    bOKToAdd = this.confirmDeletion(formElement, name, value);
                }
                if (!bOKToAdd) {
                    bOKToAdd = IsMatch("^adddefault", name);
                }
                break;
            case SharedUtils.SERVERSUBACTION_submitformedits:
                bOKToAdd = this.lastCharIsMatch(value);
                if (this.isDeletion(value)) {
                    bOKToAdd = this.confirmDeletion(formElement, name, value);
                }
                break;
            case SharedUtils.SERVERSUBACTION_saveselects:
                if (IsMatch("^true", value)) {
                    bOKToAdd = true;
                }
                break;
            case SharedUtils.SERVERSUBACTION_buildtempdoc:
                if (name == "rdobuildselects") {
                    if (formElement.checked) {
                        bOKToAdd = true;
                    }
                }
                else if (name == "rdoselects") {
                    if (formElement.checked) {
                        bOKToAdd = true;
                    }
                }
                break;
            case SharedUtils.SERVERSUBACTION_adddefaults:
                bOKToAdd = IsMatch("^adddefault", name);
                break;
            case SharedUtils.SERVERSUBACTION_runaddin:
                //addins use edited form elements along with second xml docs
                bOKToAdd = this.lastCharIsMatch(value);
                break;
            default:
                if (value == "true") {
                    bOKToAdd = true;
                }
                else if (value == "SelectedFile") {
                    //checklists that have names ending in "*" are treated separately
                    //this is strictly for stateful selections
                    bOKToAdd = this.lastCharIsMatch(name);
                }
                if (name == "historytoc") {
                    bOKToAdd = true;
                }
                break;
        }
        if (bOKToAdd) {
            sFormElement = this.addFormElement(this.encodeFormElement(name), this.encodeFormElement(value));
        }
        return sFormElement;
    },
    addOtherElements: function (params, serveraction, subaction) {
        var sOtherElements = "";
        var value = "";
        if (serveraction == SharedUtils.SERVERACTION_edit) {
            //form being submitted is the select form, it doesn't have these els
            if (subaction == SharedUtils.SERVERSUBACTION_saveselects) {
                if ($("#selectsnodeuripattern").length > 0) {
                    value = $("#selectsnodeuripattern").val();
                    sOtherElements = this.addFormElement("selectsnodeuripattern", value);
                }
                if ($("#selectsuripattern").length > 0) {
                    value = $("#selectsuripattern").val();
                    sOtherElements += this.addFormElement("selectsuripattern", value);
                }
                if ($("#selectionsnodeneededname").length > 0) {
                    value = $("#selectionsnodeneededname").val();
                    sOtherElements += this.addFormElement("selectionsnodeneededname", value);
                }
                if ($("#selectsattributename").length > 0) {
                    value = $("#selectsattributename").val();
                    sOtherElements += this.addFormElement("selectsattributename", value);
                }
                if ($("#selectscalcparams").length > 0) {
                    value = $("#selectscalcparams").val();
                    bOKToAdd = this.paramIsOKToAdd(params, value);
                    if (bOKToAdd) {
                        sOtherElements += this.addFormElement("selectscalcparams", value);
                    }
                }
            }
            else if (subaction == SharedUtils.SERVERSUBACTION_buildtempdoc) {
                //build new doc uses a hidden input to store selections made on preview panel
                if ($("#selectionsmade").length > 0) {
                    value = $("#selectionsmade").val();
                    sOtherElements += this.addFormElement("selectionsmade", value);
                }
            }
        }
        return sOtherElements;
    },
    paramIsOKToAdd: function (params, value) {
        var bOKToAdd = true;
        //check to make sure the params doesn't already have the value
        //note: this assumes that only one calcparam is in value (&typeid=17)
        if (value != "") {
            var sParam = GetSubStringWithDelimiter(value, SharedUtils.EqualDelimiter, 0);
            if (sParam != "") {
                iIndex = params.indexOf(sParam);
                if (iIndex != -1) {
                    bOKToAdd = false;
                }
            }
        }
        return bOKToAdd;
    },
    lastCharIsMatch: function (value) {
        var bOKToAdd = false;
        if (value != null && value != undefined) {
            var iLength = value.length - 1;
            if (iLength > 0) {
                var sLastChar = value.charAt(iLength);
                if (sLastChar == "*") {
                    bOKToAdd = true;
                }
            }
        }
        return bOKToAdd;
    },
    isDeletion: function (value) {
        var bIsDeletion = IsMatch("^Delete", value);
        if (!bIsDeletion) {
            bIsDeletion = IsMatch("^delete", value);
        }
        return bIsDeletion;
    },
    confirmDeletion: function (formElement, name, value) {
        var bOKToAdd = true;
        var bIsDeletion = this.isDeletion(value);
        if (bIsDeletion) {
            if (formElement.checked) {
                if (window.confirm("Are you sure you want to delete: " + name)) {
                    bOKToAdd = true;
                    //hide panels, so user can't select a deleted node
                    var oChangeTabViewer = new ChangeTabView.ViewChanger();
                    oChangeTabViewer.hideAfterDeletes();
                }
                else {
                    bOKToAdd = false;
                    //uncheck the form element
                    $(formElement).removeProp("checked");
                }
            }
            else {
                bOKToAdd = false;
            }
        }
        return bOKToAdd;
    },
    encodeFormElement: function (name) {
        //temp encode chars that interfere with form elements
        var sName = "";
        //properties with values that don't get edited
        var bHasSubString = HasSubString(name, "MathExpression");
        var bHasSubString2 = HasSubString(name, "JointDataURL");
        if (!bHasSubString && !bHasSubString2) {
            if (name != null && name != undefined) {
                sName = name.replace(/&/gi, "");
                sName = sName.replace(/=/gi, "");
                //jquery removes from mathexpress
                sName = sName.replace(/\+/gi, "--");
            }
        }
        else {
            sName = name;
        }
        return sName;
    },
    getRowParams: function (oldStartRow, startRow, isForward, networkFilterType, parentStartRow) {
        var sRowParams = this.addFormElement("oldstartrow", oldStartRow);
        sRowParams += this.addFormElement("startrow", startRow);
        sRowParams += this.addFormElement("isforward", isForward);
        sRowParams += this.addFormElement("networkfiltertype", networkFilterType);
        sRowParams += this.addFormElement("parentstartrow", parentStartRow);
        return sRowParams;
    },
    appendFormInputEl: function (parentElJQryName, name, value) {
        var $parentEl = $(parentElJQryName);
        if ($parentEl.length > 0) {
            $parentEl.append(MakeInputString("hidden", "", name, "", value, false));
        }
    }
}