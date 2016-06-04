using DevTreks.Data;
using DevTreks.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using DataHelpers = DevTreks.Data.Helpers.GeneralHelpers;

namespace DevTreks.Helpers
{
    /// <summary>
    ///Purpose:		Helper class for viewdata classes
    ///Author:		www.devtreks.org
    ///Date:		2016, April
    ///References:	www.devtreks.org
    /// </summary>
    public class ViewDataHelper
    {
        //constructor news this then InitModelProps:  
        //1. transfers Startup config settings from initURI to _uri
        //2. sets paths from context in _uri
        private ContentURI _uri {get; set;}

        public const string SEARCH_VIEW = "Search";
        public const string SEARCH_AJAXRESULT_VIEW = "_SearchBody";
        public const string CONTENT_VIEW = "Content";
        public const string CONTENT_AJAXRESULT_VIEW = "_ContentBody";
        public const string ERROR_VIEW = "Error";
        public const string MESSAGE_VIEW = "_Message";
        public const string JSON_VIEW = "_Json";
        private const string CONTROLLER = "controller";
        private const string ACTION = "action";
        //variable route
        public const string CONTENTURIPATTERN = "contenturipattern";
        //variable query string
        public const string QRYPARAMS = "qryparams";
        ////acceptable query string and form el parameters (usually ajax partial views)
        ////main contenturipattern passed between client and server (controller/action/network...)
        public const string CONTENT_PATTERN = "cp";
        ////client scripts add these to forms collection to help display 
        ////ajax responses (they may be deprecated in future releases)
        public const string CLIENT_ACTION = "ca";
        
        public ViewDataHelper()
        {
            _uri = new ContentURI();
        }
        public void GetNewAction(HttpContext context,
            ref DataHelpers.SERVER_ACTION_TYPES serverActionType)
        {
            //url started with one action, but the ajax request might be for another ...
            string sNewContentURIPattern = GetRequestParam(context, CONTENT_PATTERN);
            if (string.IsNullOrEmpty(sNewContentURIPattern))
            {
                sNewContentURIPattern = GetRequestParam(context, Helpers.IOHelper.FILE_UPLOAD_URI);
            }
            if (!string.IsNullOrEmpty(sNewContentURIPattern))
            {
                //don't build a ContentURI because networkname uses a db hit
                string controller = string.Empty;
                string action = string.Empty;
                string networkName = string.Empty;
                string nodeName = string.Empty;
                string name = string.Empty;
                string id = string.Empty;
                string fileNameExtensionType = string.Empty;
                string subAction = string.Empty;
                string subActionView = string.Empty;
                string variable = string.Empty;
                ContentURI.GetContentURIParams(sNewContentURIPattern,
                    out controller, out action,
                    out networkName, out nodeName, out name,
                    out id, out fileNameExtensionType, out subAction,
                    out subActionView, out variable);
                if (!string.IsNullOrEmpty(action))
                {
                    serverActionType = DataHelpers.GetServerAction(action);
                }
            }
        }
        public static void GetRouteValues(Microsoft.AspNetCore.Routing.RouteData route, out string controller, 
            out string action, out string partialUriPattern)
        {
            controller = (route.Values[CONTROLLER] != null) ?
                route.Values[CONTROLLER].ToString() : DataHelpers.GetDefaultNetworkGroupName();
            action = (route.Values[ACTION] != null) ?
                route.Values[ACTION].ToString() : DataHelpers.SERVER_ACTION_TYPES.search.ToString();
            partialUriPattern = (route.Values[CONTENTURIPATTERN] != null) ?
                route.Values[CONTENTURIPATTERN].ToString() : DataHelpers.MakeURIPatternStart();
        }
        public static void GetViews(Microsoft.AspNetCore.Routing.RouteData route, 
            HttpContext context, string controller, 
            out bool isInitView, ref string viewName, 
            ref string title)
        {
            isInitView = false;
            bool bIsAjaxSubaction = IsAjaxSubaction(context);
            title = GetControllerTitle(controller);
            if (bIsAjaxSubaction)
            {
                //if needed, can do a switch based on subaction type
                //for now, this is simple enough
                if (viewName.StartsWith(SEARCH_VIEW))
                {
                    viewName = SEARCH_AJAXRESULT_VIEW;
                }
                else
                {
                    viewName = CONTENT_AJAXRESULT_VIEW;
                }
            }
            else
            {
                //init view
                isInitView = true;
            }
        }
        private static string GetControllerTitle(string controllerId)
        {
            string sControllerTitle = string.Empty;
            string sControllerIdLow = controllerId.ToLower();
            if (sControllerIdLow ==
                DevTreks.Controllers.CommonTreksController.COMMON_TREKS.ToLower())
            {
                sControllerTitle = AppHelper.GetResource("COMMONTREKS_TITLE");
            }
            else if (sControllerIdLow ==
                DevTreks.Controllers.AgTreksController.AG_TREKS.ToLower())
            {
                sControllerTitle = AppHelper.GetResource("AGTREKS_TITLE");
            }
            else if (sControllerIdLow ==
                DevTreks.Controllers.BuildTreksController.BUILD_TREKS.ToLower())
            {
                sControllerTitle = AppHelper.GetResource("BUILDTREKS_TITLE");
            }
            else if (sControllerIdLow ==
                DevTreks.Controllers.HealthTreksController.HEALTH_TREKS.ToLower())
            {
                sControllerTitle = AppHelper.GetResource("HOMETREKS_TITLE");
            }
            else if (sControllerIdLow ==
                DevTreks.Controllers.HomeTreksController.HOME_TREKS.ToLower())
            {
                sControllerTitle = AppHelper.GetResource("HOMETREKS_TITLE");
            }
            else if (sControllerIdLow ==
                DevTreks.Controllers.GreenTreksController.GREEN_TREKS.ToLower())
            {
                sControllerTitle = AppHelper.GetResource("GREENTREKS_TITLE");
            }
            else if (sControllerIdLow ==
                DevTreks.Controllers.GovTreksController.GOV_TREKS.ToLower())
            {
                sControllerTitle = AppHelper.GetResource("GOVTREKS_TITLE");
            }
            return sControllerTitle;
        }
        public static void CheckForNewViews(ContentURI uri, ref string viewName)
        {
            if (string.IsNullOrEmpty(uri.ErrorMessage) == false)
            {
                //refactored out in beta 1.5
                //all panels display custom error messages at top
                //viewName = ERROR_VIEW;
            }
            else if (string.IsNullOrEmpty(uri.Message) == false)
            {
                viewName = MESSAGE_VIEW;
            }
            else if (string.IsNullOrEmpty(uri.Json) == false)
            {
                viewName = JSON_VIEW;
            }
        }
        public ContentURI SetInitialModelProperties(
            ContentURI initialURI, HttpContext context, 
            string controller, string action, string partialUriPattern)
        {
            //set any params sent in request body
            SetModel(context, controller, action, partialUriPattern);
            //set misc other params sent by client 
            SetModelPropsFromForm();
            //set apps, subapps and servicegroups
            DataHelpers.SetApps(_uri);

            //copy appsettings and configs from initialuri to _uri
            DevTreks.Data.Helpers.AppSettings.CopyURIAppSettings(initialURI, _uri);
            //set additional appsettings from request (should be done with context after)
            //absolute web path: comes from ms.aspnet.http.extensions
            //http://localhost/greentreks/search/etc...
            DevTreks.Data.Helpers.AppSettings.SetAbsoluteURLPath(_uri, context.Request.GetDisplayUrl());
            //context.Request.GetEncodedUrl() can also be used, but no difference with these urls

            //NOTE: If Networks are used to store data in separate databases,
            //use _uri.URINetworkPartName or _uri.URINetwork to find the corresponding 
            //connection string from usersecrets and instead of storing all content in 
            //commontreks, store it the same as resources (i.e. network_ag .., but admin pattern must be changed too)

            return _uri;
        }

        public void SetModel(HttpContext context,
            string controller, string action, string partialContentUriPattern)
        {
            
            string sNewContentURIPattern = string.Empty;
            //either of these two qry/form params can change _uri (from initial partialUriPattern)
            //i.e. button click events that change initial partialUriPattern
            //these are full pattern: controller/action/network/...
            sNewContentURIPattern = GetRequestParam(context, CONTENT_PATTERN);
            if (!string.IsNullOrEmpty(sNewContentURIPattern))
            {
                _uri = new ContentURI(sNewContentURIPattern);
                //new uri needs form params
                AddFormParams(context);
                AddQueryStringParams(context);
                SetModelParams();
                return;
            }
            string sNewContentURIPatternFU = GetRequestParam(context, Helpers.IOHelper.FILE_UPLOAD_URI);
            if (!string.IsNullOrEmpty(sNewContentURIPatternFU))
            {
                _uri = new ContentURI(sNewContentURIPatternFU); 
                //new uri needs form params
                AddFormParams(context);
                AddQueryStringParams(context);
                SetModelParams();
            }
            else
            {
                //this is either not an ajax request, or it is an ajax 
                //request received from an ajax link - the 
                //partialUriPattern does not have a controller or action yet
                //(they are automatically put in the route values)
                sNewContentURIPattern = ContentURI.GetFullContentURIFromPartial(
                    partialContentUriPattern, controller, action);
                _uri = new ContentURI(sNewContentURIPattern);
                //default _uri is based on partialUriPattern in route
                AddFormParams(context);
                AddQueryStringParams(context);
                SetModelParams();
            }
        }
        private void SetModelParams()
        {
            //rare, but qry string and form params can change these params
            _uri.URIDataManager.ClientActionType = (!string.IsNullOrEmpty(DataHelpers.GetFormValue(_uri, CLIENT_ACTION)))
                    ? (DataHelpers.CLIENTACTION_TYPES)Enum.Parse(typeof(DataHelpers.CLIENTACTION_TYPES), 
                    DataHelpers.GetFormValue(_uri, CLIENT_ACTION)) : DataHelpers.CLIENTACTION_TYPES.postrequest;
            //this assumes a 1-1 relation between serveractiontype and updatepaneltype
            //in the current mvc pattern, that's true, but if it becomes false
            //add an updatepanel type argument to the ajax client click event 
            _uri.URIDataManager.UpdatePanelType = (DataHelpers.UPDATE_PANEL_TYPES)_uri.URIDataManager.ServerActionType;
            
        }
        private DataHelpers.SERVER_ACTION_TYPES GetServerActionTypeFromRoute(Microsoft.AspNetCore.Routing.RouteData route)
        {
            DataHelpers.SERVER_ACTION_TYPES eActionType
                = DataHelpers.SERVER_ACTION_TYPES.preview;
            if (route != null)
            {
                string sActionName = route.Values[ACTION].ToString().ToLower();
                eActionType = (string.IsNullOrEmpty(sActionName)) 
                    ? DataHelpers.SERVER_ACTION_TYPES.preview : (DataHelpers.SERVER_ACTION_TYPES)Enum.Parse(typeof(DataHelpers.SERVER_ACTION_TYPES), sActionName);
            }
            else
            {
                //tests end up here 
                eActionType = DataHelpers.SERVER_ACTION_TYPES.search;
            }
            return eActionType;
        }
        public static string GetRequestParam(
            HttpContext context, string paramName)
        {
            string sRequestParam = string.Empty;
            if (context.Request != null)
            {
                //check for form post params
                if (context.Request.HasFormContentType)
                {
                    if (context.Request.Form.Count != 0)
                    {
                        sRequestParam = context.Request.Form[paramName];
                        if (sRequestParam == null)
                            sRequestParam = string.Empty;
                    }
                }
                if (sRequestParam == string.Empty)
                {
                    if (context.Request.Query != null)
                    {
                        if (context.Request.Query.Count != 0)
                        {
                            sRequestParam = context.Request.Query[paramName];
                            if (sRequestParam == null)
                                sRequestParam = string.Empty;
                        }
                    }
                }
            }
            return sRequestParam;
        }
        private void AddFormParams(HttpContext context)
        {
            if (context.Request != null)
            {
                //check for form post params
                if (context.Request.HasFormContentType)
                {
                    if (context.Request.Form.Count != 0)
                    {
                        AddFormCollectionToFormInput(_uri, context.Request.Form);
                    }
                }
            }
        }
        public static void AddFormCollectionToFormInput(ContentURI uri,
            IFormCollection postBackParams)
        {
            if (postBackParams != null)
            {
                if (uri.URIDataManager.FormInput == null)
                {
                    uri.URIDataManager.FormInput = new Dictionary<string, string>();
                }
                string sValue = string.Empty;
                string sKey = string.Empty;
                foreach (var keyVP in postBackParams)
                {
                    sKey = keyVP.Key;
                    sValue = keyVP.Value;
                    if (sValue != null && sValue != "undefined")
                        uri.URIDataManager.FormInput.Add(sKey, sValue);
                }
            }
        }
        private void AddQueryStringParams(HttpContext context)
        {
            if (context.Request != null)
            {
                //add request.querystring params
                if (context.Request.Query != null)
                {
                    AddQueryCollectionToFormInput(_uri, context.Request.Query);
                }
            }
        }
        public static void AddQueryCollectionToFormInput(ContentURI uri,
            IQueryCollection postBackParams)
        {
            if (postBackParams != null)
            {
                if (uri.URIDataManager.FormInput == null)
                {
                    uri.URIDataManager.FormInput = new Dictionary<string, string>();
                }
                string sKey = string.Empty;
                string sValue = string.Empty;
                foreach (var keyVP in postBackParams)
                {
                    sKey = keyVP.Key;
                    sValue = keyVP.Value;
                    //2.0.0 added the undefined condition
                    if (sValue != null && sValue != "undefined")
                        uri.URIDataManager.FormInput.Add(sKey, sValue);
                }
            }
        }
        
        public void SetModelPropsFromForm()
        {
            if (_uri.URIDataManager.FormInput != null)
            {
                string sSelectionsNodeNeededName 
                    = DataHelpers.GetFormValue(_uri, 
                    DevTreks.Data.EditHelpers.AddHelperLinq.SELECT_EXISTING_PARAMS.selectionsnodeneededname.ToString());
                _uri.URIDataManager.SelectionsNodeNeededName 
                    = (sSelectionsNodeNeededName == null) 
                    ? string.Empty : sSelectionsNodeNeededName;
            }
            else
            {
                _uri.URIDataManager.SelectionsNodeNeededName = string.Empty;
            }
        }
        public static void GetRowArgs(Dictionary<string, string> rowArgs,
            int pageSize, out int startRow, out string isForward, out int parentStartRow)
        {
            if (rowArgs != null)
            {
                string sOldStartRow = DataHelpers.GetFormValue(rowArgs, StylesheetHelper.START_ROW_TYPES.oldstartrow.ToString());
                int iOldStartRow = (string.IsNullOrEmpty(sOldStartRow)) ? 0 : DataHelpers.ConvertStringToInt(sOldStartRow);
                string sStartRow = DataHelpers.GetFormValue(rowArgs, StylesheetHelper.START_ROW_TYPES.startrow.ToString());
                startRow = (string.IsNullOrEmpty(sStartRow)) ? 0 : DataHelpers.ConvertStringToInt(sStartRow);
                //0=back; 1=forward
                isForward = DataHelpers.GetFormValue(rowArgs, StylesheetHelper.START_ROW_TYPES.isforward.ToString());
                isForward = (string.IsNullOrEmpty(isForward)) ? "-1" : isForward;
                //pageSize = 0;
                //do some rules here
                GetStartRow(iOldStartRow, isForward, pageSize, ref startRow);
                if (isForward == "-1")
                {
                    //search records will return startrow + pagesize records
                    isForward = "1";
                }
                //parent toc clicks return to same starting row and when children were clicked
                string sParentStartRow = DataHelpers.GetFormValue(rowArgs, StylesheetHelper.START_ROW_TYPES.parentstartrow.ToString());
                parentStartRow = (string.IsNullOrEmpty(sParentStartRow)) ? 0 : DataHelpers.ConvertStringToInt(sParentStartRow);
            }
            else
            {
                startRow = 0;
                isForward = "1";
                //pageSize = 0;
                parentStartRow = 0;
            }
        }
        
        private static void GetStartRow(int oldStartRow, string isForward, 
            int pageSize, ref int startRow)
        {
            int iNewStartRow = 0;
            //see if the user changed the start row
            if (startRow != oldStartRow)
            {
                iNewStartRow = startRow;
            }
            else
            {
                //change it based on forward or upwards action
                int iStartRow = 0;
                if (isForward == "1")
                {
                    iStartRow = oldStartRow + pageSize;
                }
                else if (isForward == "0")
                {
                    iStartRow = oldStartRow - pageSize;
                }
                else
                {
                    //don't change it
                    iStartRow = oldStartRow;
                }
                if (iStartRow < 0) iStartRow = 0;
                iNewStartRow = iStartRow;
            }
            startRow = iNewStartRow;
        }
        public static string GetNetworkGroupFromRoute(
            Microsoft.AspNetCore.Routing.RouteData route, HttpContext context)
        {
            string sNetworkGroupName = string.Empty;
            if (route != null)
            {
                sNetworkGroupName = (!string.IsNullOrEmpty(route.Values[CONTROLLER].ToString())) ?
                    route.Values[CONTROLLER].ToString().ToLower()
                    : DataHelpers.GetDefaultNetworkGroupName();
            }
            else
            {
                sNetworkGroupName = DataHelpers.GetDefaultNetworkGroupName();
            }
            return sNetworkGroupName;
        }
        public static bool GetContentAction(DataHelpers.SERVER_ACTION_TYPES serverActionType)
        {
            bool bUseContentAction = true;
            if (serverActionType == DevTreks.Data.Helpers.GeneralHelpers.SERVER_ACTION_TYPES.search)
            {
                bUseContentAction = false;

            }
            return bUseContentAction;
        }
        public static bool IsAjaxSubaction(HttpContext context)
        {
            bool bIsAjaxSubaction = false;
            //if a serversubaction is specified in the request's querystring
            //it's an ajax postback
            if (context.Request.QueryString != null)
            {
                if (context.Request.Query != null)
                {
                    if (context.Request.Query.Any(k => k.Key == CONTENT_PATTERN))
                    {
                        //DevTreks GET ajax requests always include this parameter in qry string
                        bIsAjaxSubaction = true;
                    }
                    else if (context.Request.Headers.Any(k => k.Key == "X-Requested-With"))
                    {
                        //MVC ajax requests include this variable
                        bIsAjaxSubaction = true;
                        //or more likely to be
                        //var isAjax = request.Headers["X-Requested-With"] == "XMLHttpRequest"
                    }
                }
            }
            if (!bIsAjaxSubaction)
            {
                if (context.Request.HasFormContentType)
                {
                    if (context.Request.Form.Any(k => k.Key == CONTENT_PATTERN))
                    {
                        //DevTreks POST ajax requests always include this parameter in qry string
                        bIsAjaxSubaction = true;
                    }
                }
            }
            return bIsAjaxSubaction;
        }
        
    }
}
