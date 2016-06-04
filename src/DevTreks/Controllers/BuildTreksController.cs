using DevTreks.Services;
using DevTreks.Helpers;
using DevTreks.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using DataHelpers = DevTreks.Data.Helpers.GeneralHelpers;

namespace DevTreks.Controllers
{
    /// <summary>
    ///Purpose:		Manages services and models for the buildtreks views
    ///Author:		www.devtreks.org
    ///Date:		2016, March
    ///References:	
    /// </summary>
    public class BuildTreksController : Controller, IDevTreksController1
    {
        public const string BUILD_TREKS = "BuildTreks";
        private ISearchService _searchService { get; set; }
        private IMemberService _memberService { get; set; }
        private IContentService _contentService { get; set; }
        private DevTreks.Data.ContentURI _uri { get; set; }
        private ILogger<BuildTreksController> _logger { get; set; }
        //2.0.0 changes
        public BuildTreksController(IOptions<DevTreks.Data.ContentURI> uri,
            ILogger<BuildTreksController> logger)
        {
            _uri = new DevTreks.Data.ContentURI();
            _uri = new DevTreks.Data.ContentURI(uri.Value);
            _searchService = new SearchService(_uri);
            _memberService = new MemberService(_uri);
            _contentService = new ContentService(_uri);
            _logger = logger;
        }
        public async Task<ActionResult> Index()
        {
            if (this.Request.Method != "POST")
            {
                ViewData["Title"] = AppHelper.GetResource("BUILDTREKS_TITLE");
                return View();
            }
            else
            {
                //jquery.ajax form posts to absolute path work around
                string sContenturipattern = string.Empty;
                DataHelpers.SERVER_ACTION_TYPES eServerActionType
                    = DataHelpers.SERVER_ACTION_TYPES.edit;
                ViewDataHelper viewHelper = new ViewDataHelper();
                viewHelper.GetNewAction(this.HttpContext, ref eServerActionType);
                return await this.GeneralAction(sContenturipattern, eServerActionType);
            }
        }
        [HttpGet]
        public async Task<ActionResult> Search(string contenturipattern)
        {
            contenturipattern = (string.IsNullOrEmpty(contenturipattern)) ?
                DataHelpers.MakeURIPatternStart() : contenturipattern;
            //check for redirections from ajax content requests or subserveractiontypes
            DataHelpers.SERVER_ACTION_TYPES eServerActionType
                = DataHelpers.SERVER_ACTION_TYPES.search;
            ViewDataHelper viewHelper = new ViewDataHelper();
            viewHelper.GetNewAction(this.HttpContext, ref eServerActionType);
            return await this.GeneralAction(contenturipattern, eServerActionType);
        }
        public async Task<ActionResult> GeneralAction(string contenturipattern,
            DataHelpers.SERVER_ACTION_TYPES serverActionType)
        {
            bool bUseContentAction = ViewDataHelper.GetContentAction(serverActionType);
            bool bIsAjaxSubaction = ViewDataHelper.IsAjaxSubaction(this.Request.HttpContext);
            if (bUseContentAction == true)
            {
                if (bIsAjaxSubaction)
                {
                    return await this.ContentAjaxAction(contenturipattern);
                }
                else
                {
                    return await this.ContentAction(contenturipattern);
                }
            }
            else
            {
                if (bIsAjaxSubaction)
                {
                    return await this.SearchAjaxAction(contenturipattern);
                }
                else
                {
                    return await this.SearchAction(contenturipattern);
                }
            }
        }
        public async Task<ActionResult> SearchAction(string contenturipattern)
        {
            string sViewName = ViewDataHelper.SEARCH_VIEW;
            string sTitle = string.Empty;
            ViewData["NetworkGroup"] = BUILD_TREKS;
            //construct the viewdata model
            SearchViewModel vwSearchViewModel = new SearchViewModel(_uri);
            try
            {
                bool bHasSet = await vwSearchViewModel.SetViewAsync(this.RouteData, this.Request.HttpContext,
                        _searchService, _memberService, contenturipattern);
                sViewName = vwSearchViewModel.ViewName;
                sTitle = vwSearchViewModel.Title;
                ViewData["Title"] = sTitle;
                ViewData["URI"] = vwSearchViewModel.SearchManagerData.SearchResult.URIFull
                    ?? string.Empty;
                //return the model to the view
                return View(sViewName, vwSearchViewModel);
            }
            catch (Exception x)
            {
                AppHelper.SetErrorMessage(x, vwSearchViewModel.SearchManagerData.SearchResult);
                //return the model to the view
                return View(sViewName, vwSearchViewModel);
            }
        }
        public async Task<PartialViewResult> SearchAjaxAction(string contenturipattern = "")
        {
            string sViewName = ViewDataHelper.SEARCH_AJAXRESULT_VIEW;
            string sTitle = string.Empty;
            ViewData["NetworkGroup"] = BUILD_TREKS;
            //construct the viewdata model
            SearchViewModel vwSearchViewModel = new SearchViewModel(_uri);
            try
            {
                bool bHasSet = await vwSearchViewModel.SetViewAsync(this.RouteData, this.Request.HttpContext,
                        _searchService, _memberService, contenturipattern);
                sViewName = vwSearchViewModel.ViewName;
                sTitle = vwSearchViewModel.Title;
                ViewData["Title"] = sTitle;
                ViewData["URI"] = vwSearchViewModel.SearchManagerData.SearchResult.URIFull
                    ?? string.Empty;
                //return the model to the view
                return PartialView(sViewName, vwSearchViewModel);
            }
            catch (Exception x)
            {
                AppHelper.SetErrorMessage(x, vwSearchViewModel.SearchManagerData.SearchResult);
                //return the model to the view
                return PartialView(sViewName, vwSearchViewModel);
            }
        }
        public async Task<ActionResult> Preview(string contenturipattern)
        {
            contenturipattern = (string.IsNullOrEmpty(contenturipattern)) ? DataHelpers.MakeURIPatternStart()
                : contenturipattern;
            DataHelpers.SERVER_ACTION_TYPES eServerActionType
                = DataHelpers.SERVER_ACTION_TYPES.preview;
            ViewDataHelper viewHelper = new ViewDataHelper();
            viewHelper.GetNewAction(this.HttpContext, ref eServerActionType);
            return await this.GeneralAction(contenturipattern, eServerActionType);
        }
        [HttpGet]
        public async Task<ActionResult> Select(string contenturipattern)
        {
            contenturipattern = (string.IsNullOrEmpty(contenturipattern)) ? DataHelpers.MakeURIPatternStart()
                : contenturipattern;
            DataHelpers.SERVER_ACTION_TYPES eServerActionType
                = DataHelpers.SERVER_ACTION_TYPES.select;
            ViewDataHelper viewHelper = new ViewDataHelper();
            viewHelper.GetNewAction(this.HttpContext, ref eServerActionType);
            return await this.GeneralAction(contenturipattern, eServerActionType);
        }
        [HttpGet]
        public async Task<ActionResult> Edit(string contenturipattern)
        {
            contenturipattern = (string.IsNullOrEmpty(contenturipattern)) ? DataHelpers.MakeURIPatternStart()
                : contenturipattern;
            DataHelpers.SERVER_ACTION_TYPES eServerActionType
                = DataHelpers.SERVER_ACTION_TYPES.edit;
            ViewDataHelper viewHelper = new ViewDataHelper();
            viewHelper.GetNewAction(this.HttpContext, ref eServerActionType);
            return await this.GeneralAction(contenturipattern, eServerActionType);
        }
        [HttpGet]
        public async Task<ActionResult> Pack(string contenturipattern)
        {
            contenturipattern = (string.IsNullOrEmpty(contenturipattern)) ? DataHelpers.MakeURIPatternStart()
                : contenturipattern;
            DataHelpers.SERVER_ACTION_TYPES eServerActionType
                = DataHelpers.SERVER_ACTION_TYPES.pack;
            ViewDataHelper viewHelper = new ViewDataHelper();
            viewHelper.GetNewAction(this.HttpContext, ref eServerActionType);
            return await this.GeneralAction(contenturipattern, eServerActionType);
        }
        [HttpGet]
        public async Task<ActionResult> Member(string contenturipattern)
        {
            contenturipattern = (string.IsNullOrEmpty(contenturipattern)) ? DataHelpers.MakeURIPatternStart()
                : contenturipattern;
            DataHelpers.SERVER_ACTION_TYPES eServerActionType
                = DataHelpers.SERVER_ACTION_TYPES.member;
            ViewDataHelper viewHelper = new ViewDataHelper();
            viewHelper.GetNewAction(this.HttpContext, ref eServerActionType);
            return await this.GeneralAction(contenturipattern, eServerActionType);
        }
        public async Task<ActionResult> LinkedViews(string contenturipattern)
        {
            contenturipattern = (string.IsNullOrEmpty(contenturipattern)) ? DataHelpers.MakeURIPatternStart()
                : contenturipattern;
            DataHelpers.SERVER_ACTION_TYPES eServerActionType
                = DataHelpers.SERVER_ACTION_TYPES.linkedviews;
            ViewDataHelper viewHelper = new ViewDataHelper();
            viewHelper.GetNewAction(this.HttpContext, ref eServerActionType);
            return await this.GeneralAction(contenturipattern, eServerActionType);
        }
        public async Task<ActionResult> ContentAction(string contenturipattern = "")
        {
            contenturipattern = (string.IsNullOrEmpty(contenturipattern)) ?
                DataHelpers.MakeURIPatternStart() : contenturipattern;
            string sViewName = ViewDataHelper.CONTENT_VIEW;
            string sTitle = string.Empty;
            ViewData["NetworkGroup"] = BUILD_TREKS;
            //construct the viewdata model
            ContentViewModel vwContentViewModel = new ContentViewModel(_uri);
            try
            {
                bool bHasSet = await vwContentViewModel.SetViewAsync(this.RouteData, this.Request.HttpContext,
                        _contentService, _memberService, contenturipattern);
                sViewName = vwContentViewModel.ViewName;
                sTitle = vwContentViewModel.Title;
                ViewData["Title"] = sTitle;
                ViewData["URI"] = vwContentViewModel.ContentURIData.URIFull ?? string.Empty;
                //return the model to the view
                if (sViewName == ViewDataHelper.MESSAGE_VIEW)
                {
                    //respond with the message
                    ViewData[ViewDataHelper.MESSAGE_VIEW] = vwContentViewModel.ContentURIData.Message;
                    return View(sViewName);
                }
                else if (sViewName == ViewDataHelper.ERROR_VIEW)
                {
                    //respond with the error message
                    ViewData[ViewDataHelper.ERROR_VIEW] = vwContentViewModel.ContentURIData.ErrorMessage;
                    return View(sViewName);
                }
                else if (sViewName == ViewDataHelper.JSON_VIEW)
                {
                    //respond with the javascript
                    return Json(vwContentViewModel.ContentURIData.Json);
                }
                //respond with the model
                return View(sViewName, vwContentViewModel);
            }
            catch (Exception x)
            {
                AppHelper.SetErrorMessage(x, vwContentViewModel.ContentURIData);
                //respond with the model
                return View(sViewName, vwContentViewModel);
            }
        }
        public async Task<PartialViewResult> ContentAjaxAction(string contenturipattern = "")
        {
            string sViewName = ViewDataHelper.CONTENT_AJAXRESULT_VIEW;
            string sTitle = string.Empty;
            ViewData["NetworkGroup"] = BUILD_TREKS;
            //construct the viewdata model
            ContentViewModel vwContentViewModel = new ContentViewModel(_uri);
            try
            {
                bool bHasSet = await vwContentViewModel.SetViewAsync(this.RouteData, this.Request.HttpContext,
                        _contentService, _memberService, contenturipattern);
                sViewName = vwContentViewModel.ViewName;
                sTitle = vwContentViewModel.Title;
                ViewData["Title"] = sTitle;
                ViewData["URI"] = vwContentViewModel.ContentURIData.URIFull ?? string.Empty;
                //return the model to the view
                if (sViewName == ViewDataHelper.CONTENT_VIEW)
                {
                    return PartialView(sViewName, vwContentViewModel);
                }
                else
                {
                    return PartialView(sViewName, vwContentViewModel.ContentURIData);
                }
            }
            catch (Exception x)
            {
                AppHelper.SetErrorMessage(x, vwContentViewModel.ContentURIData);
                //return the model to the view
                if (sViewName == ViewDataHelper.CONTENT_VIEW)
                {
                    return PartialView(sViewName, vwContentViewModel);
                }
                else
                {
                    return PartialView(sViewName, vwContentViewModel.ContentURIData);
                }
            }
        }
        //mvc automatically calls
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Free managed resources.
                if (_searchService != null)
                {
                    _searchService.Dispose();
                }
                if (_contentService != null)
                {
                    _contentService.Dispose();
                }
                if (_memberService != null)
                {
                    _memberService.Dispose();
                }
                base.Dispose(disposing);
            }
        }
    }
}
