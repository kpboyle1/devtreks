/*
Name:       DevTreksJQ1.js
Author:     www.devtreks.org
Date:       2013, January
Purpose:    JQuery 1.8.0 utils
*/
$(document).ready(function () {
    //jquery 
    $(pageReady);
});
function pageReady() {
    //set up the top tabs with appropriate props and events
    var oChangeTabViewer = new ChangeTabView.ViewChanger();
}

$(document).bind("mobileinit", function () {
    //don't use default jquery ajax page loading (use MVC)
    $.mobile.ajaxEnabled = false;
    $.mobile.ignoreContentEnabled = true;
    $.mobile.pushStateEnabled = false;
});
$(document).ready(function () {
    //tab click
    $(document).on("click", "a[id^='menu']", function (event) {
        var id = $(this).attr("id");
        var oChangeTabViewer = new ChangeTabView.ViewChanger(null, id);
    });
    $(document).on("click", "a[id^='step']", function (event) {
        var id = $(this).attr("id");
        var oChangeTabViewer = new ChangeTabView.ViewChanger(null, id);
    });
    //search click
    $(document).on("click", ".Search1Enabled", function (event) {
        var contenturipattern = $(this).attr(SharedUtils.DATA_CP);
        var clientaction = $(this).attr(SharedUtils.DATA_CA);
        var params = $(this).attr(SharedUtils.DATA_PARAMS);
        searchDevTreksOnClick(contenturipattern, clientaction, params);
    });
    $(document).on("click", ".SearchButton1Enabled", function (event) {
        var contenturipattern = $(this).attr(SharedUtils.DATA_CP);
        var clientaction = $(this).attr(SharedUtils.DATA_CA);
        var params = $(this).attr(SharedUtils.DATA_PARAMS);
        searchDevTreksOnClick(contenturipattern, clientaction, params);
    });
    //misc
    $(document).on("click", "input[id='selectscancel']", CancelSelections);
    $(document).on("click", "input[id='btnClearPack']", ClearPackage);
    $(document).on("change", "textarea", IsJQUpdated);
    $(document).on("change", "input[type!='button']", IsJQUpdated);
    $(document).on("change", "select", IsJQSelectionUpdated);
    //content click
    $(document).on("click", ".Button1Enabled", function (event) {
        var contenturipattern = $(this).attr(SharedUtils.DATA_CP);
        var clientaction = $(this).attr(SharedUtils.DATA_CA);
        var params = $(this).attr(SharedUtils.DATA_PARAMS);
        devTreksOnClick(contenturipattern, clientaction, params);
    });
    $(document).on("click", ".SubmitButton1Enabled150", function (event) {
        var contenturipattern = $(this).attr(SharedUtils.DATA_CP);
        var clientaction = $(this).attr(SharedUtils.DATA_CA);
        var params = $(this).attr(SharedUtils.DATA_PARAMS);
        devTreksOnClick(contenturipattern, clientaction, params);
    });
    $(document).on("click", ".ChangeView", function (event) {
        var contenturipattern = $(this).attr(SharedUtils.DATA_CP);
        var clientaction = $(this).attr(SharedUtils.DATA_CA);
        var params = $(this).attr(SharedUtils.DATA_PARAMS);
        devTreksOnClick(contenturipattern, clientaction, params);
        var oChangeTabViewer = new ChangeTabView.ViewChanger(null, "stepzero");
    });
    $(document).on("click", ".SubmitButton1Enabled", function (event) {
        var contenturipattern = $(this).attr(SharedUtils.DATA_CP);
        var clientaction = $(this).attr(SharedUtils.DATA_CA);
        var params = $(this).attr(SharedUtils.DATA_PARAMS);
        devTreksOnClick(contenturipattern, clientaction, params);
    });
    $(document).on("click", ".SmallButton", function (event) {
        var contenturipattern = $(this).attr(SharedUtils.DATA_CP);
        var clientaction = $(this).attr(SharedUtils.DATA_CA);
        var params = $(this).attr(SharedUtils.DATA_PARAMS);
        devTreksOnClick(contenturipattern, clientaction, params);
    });
    $(document).on("click", ".JSLink", function (event) {
        var contenturipattern = $(this).attr(SharedUtils.DATA_CP);
        var clientaction = $(this).attr(SharedUtils.DATA_CA);
        var params = $(this).attr(SharedUtils.DATA_PARAMS);
        devTreksOnClick(contenturipattern, clientaction, params);
    });
    $(document).on("click", ".Home", function (event) {
        //just show a loading message
        $.mobile.loading('show', {
            text: 'Loading ...',
            textVisible: true,
            theme: 'b',
            html: ""
        });
    });
    //selects click
    $(document).on("click", ".GetSelectsLink", function (event) {
        var spanid = $(this).attr(SharedUtils.DATA_ID1);
        var contenturipattern = $(this).attr(SharedUtils.DATA_CP);
        var nodeuripattern = $(this).attr(SharedUtils.DATA_UP1);
        var nodename = $(this).attr(SharedUtils.DATA_NODENAME);
        var attributename = $(this).attr(SharedUtils.DATA_ATTNAME);
        var params = $(this).attr(SharedUtils.DATA_PARAMS);
        getSelectionFiles(spanid, contenturipattern, nodeuripattern, nodename, attributename, params);
    });
    $(document).on("click", ".GetSelects", function (event) {
        var spanid = $(this).attr(SharedUtils.DATA_ID1);
        var contenturipattern = $(this).attr(SharedUtils.DATA_CP);
        var nodeuripattern = $(this).attr(SharedUtils.DATA_UP1);
        var nodename = $(this).attr(SharedUtils.DATA_NODENAME);
        var attributename = $(this).attr(SharedUtils.DATA_ATTNAME);
        var params = $(this).attr(SharedUtils.DATA_PARAMS);
        getSelectionFiles(spanid, contenturipattern, nodeuripattern, nodename, attributename, params);
    });
    $(document).on("click", ".GetSelections", function (event) {
        var spanid = SharedUtils.SelectionsId;
        getSelections(spanid);
    });
    $(document).on("click", "img[id!='tabsimage']", function (event) {
        var id = $(this).attr("id");
        if (id) {
            if (id == "") {
                id = $(this).attr("name");
            }
        }
        ShowThumbnail(id);
    });
    $(document).on("click", ".DeleteSelections", function (event) {
        var spanid = SharedUtils.SelectionsId;
        deleteSelections(spanid);
    });
    $(document).on("click", ".SelectUnits", function (event) {
        var unitgroupid = $(this).attr(SharedUtils.DATA_UNITID);
        SwitchUnits($(this), unitgroupid);
    });
    $(document).on("click", ".ShowElement", function (event) {
        var elid = $(this).attr(SharedUtils.DATA_ID1);
        DisplayElementByName(elid);
    });
});
$(document).ready(function () {
    $(document).on("click", ".UploadFile", function (event) {
        var buttonId = $(this).attr("id");
        if (StartsWith("btnUpload_", buttonId)
        || StartsWith("btnDisplayFileUpload_", buttonId)) {
            //client side preparation for file upload
            var contenturipattern = $(this).attr(SharedUtils.DATA_CP);
            var spanid = $(this).attr(SharedUtils.DATA_ID1);
            var nodeuripattern = $(this).attr(SharedUtils.DATA_UP1);
            var params = $(this).attr(SharedUtils.DATA_PARAMS);
            var oFileLoader = new LoadFile.FileLoader();
            oFileLoader.displayFileUploadClick(buttonId, params, spanid, contenturipattern, nodeuripattern);
        }
        else {
            //submit upload file form
            var oFileLoader = new LoadFile.FileLoader();
            oFileLoader.uploadFileClick();
        }
    });
    //load event not supported using on
    $("#frameFileUpload").bind("load", function (event) {
        var oFileLoader = new LoadFile.FileLoader();
        oFileLoader.uploadFinish();
    });
});
