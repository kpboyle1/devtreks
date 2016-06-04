using DevTreks.Helpers;
using DevTreks.Models;
using DevTreks.Services;
using DevTreks.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DevTreks.Controllers
{
    /// <summary>
    ///Purpose:		Manages services for the login, joinin, and payment handling 
    ///             portions of member view
    ///             most member editing is handled using the member action found
    ///             in agtrekscontroller, buildtrekscontroller ...
    ///Author:		www.devtreks.org
    ///Date:		2016, March
    ///References:	
    /// </summary>
   
    public class MemberController : Controller, IMemberController
    {
        private IMemberService _memberService { get; set; }
        private DevTreks.Data.ContentURI _uri { get; set; }
        private ILogger<HomeTreksController> _logger { get; set; }
        public MemberController(IMemberService memberService)
        {
            _uri = new DevTreks.Data.ContentURI();
            //used for tests
            _memberService = memberService;
            _uri = new DevTreks.Data.ContentURI();
        }
        
        public async Task<ActionResult> Payments()
        {
            MemberViewModel vwMemberViewModel = new MemberViewModel(_uri);
            try
            {
                //set the member
                bool bHasSet = await vwMemberViewModel.SetClubAndMember(this.Request.HttpContext,
                    _memberService);
                //set the clubinuse credit
                //_memberService.SetClubCredit(vwMemberViewModel.Member);
                if (TempData["errormessage"] != null)
                {
                    //update payment action failed
                    vwMemberViewModel.ErrorMessage = TempData["errormessage"].ToString();
                    if (TempData["credit"] != null)
                    {
                        //for their convenience display whatever form elements they already have filled out
                        AccountToCredit clubCredit = (AccountToCredit)TempData["credit"];
                        vwMemberViewModel.Member.ClubDefault.AccountToCredit.Add(clubCredit);
                    }
                }
                ViewData["Title"] = AppHelper.GetResource("PAYMENTS");
                return View("Payments", vwMemberViewModel);
            }
            catch (Exception x)
            {
                //AppHelper.PublishError(x, vwMemberViewModel.ErrorMessage);
                TempData["errormessage"] = x.Message;
                vwMemberViewModel.ErrorMessage = x.Message;
                return View("Payments", vwMemberViewModel);
            }
        }
        //[Authorize]
        [HttpPost]
        public async Task<ActionResult> UpdatePaymentHandling()
        {
            MemberViewModel vwMemberViewModel = new MemberViewModel(_uri);
            try
            {
                //set the member
                bool bHasSet = await vwMemberViewModel.SetClubAndMember(this.Request.HttpContext,
                    _memberService);
                //set the clubdefault credit
                //_memberService.SetClubCredit(vwMemberViewModel.Member);
                //fill in the club credit view
                Helpers.HtmlMemberExtensions.FillInClubCreditFormData(this.HttpContext,
                    vwMemberViewModel.Member);
                //update the payment info -use a new memberservice because of previous query/datacontext issues
                //if not successful, generates an error message and redirects back to same Payments view
                string sErrorMessage = string.Empty;
                sErrorMessage = await vwMemberViewModel.UpdatePaymentHandling(this.Request.HttpContext,
                    _memberService, vwMemberViewModel.Member);
                vwMemberViewModel.ErrorMessage = sErrorMessage;
                if (string.IsNullOrEmpty(vwMemberViewModel.ErrorMessage))
                {
                    //secure pages redirect to self and display success message
                    TempData["errormessage"] = "Success, the payment data has been updated.";
                }
                else
                {
                    TempData["errormessage"] = vwMemberViewModel.ErrorMessage;
                    //don't make them fill in all of the form data again
                    TempData["credit"] = vwMemberViewModel.Member.ClubDefault.AccountToCredit.FirstOrDefault();
                }
                return RedirectToAction("Payments");
            }
            catch (Exception x)
            {
                //AppHelper.PublishError(x, vwMemberViewModel.ErrorMessage);
                TempData["errormessage"] = x.Message;
                vwMemberViewModel.ErrorMessage = x.Message;
                return RedirectToAction("Payments");
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Free managed resources.
                if (_memberService != null)
                {
                    _memberService.Dispose();
                }
                base.Dispose(disposing);
            }
        }
    }
}
