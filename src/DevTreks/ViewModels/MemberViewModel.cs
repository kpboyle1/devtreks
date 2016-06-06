using DevTreks.Data;
using DevTreks.Models;
using DevTreks.Services;
using DevTreks.Helpers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Exceptions = DevTreks.Exceptions;

namespace DevTreks.ViewModels
{
    /// <summary>
    ///Purpose:		View Model class for filling in member login, joinin, and payment handling views
    ///Author:		www.devtreks.org
    ///Date:		2016, March
    ///Notes;       
    /// </summary>
    public class MemberViewModel
    {
        public MemberViewModel(ContentURI initialURI)
        {
            this.Member = new AccountToMember();
            //different use of initialURI because not as much work done here
            ContentURIData = initialURI;
        }
        public ContentURI ContentURIData { get; set; }
        public AccountToMember Member { get; set; }
        public string ErrorMessage { get; set; }
        
        public void Logout(Microsoft.AspNetCore.Http.HttpContext context)
        {
            HtmlMemberExtensions.SignOut(context);
        }
        
        public async Task<bool> SetClubAndMember(HttpContext context, 
            IMemberService memberService)
        {
            bool bHasSet = false;
            //set up initial club and member
            bool bIsInitView = true;
            MemberHelper oMemberHelper = new MemberHelper();
            bHasSet = await oMemberHelper.SetClubAndMemberAsync(context, bIsInitView, memberService,
                ContentURIData);
            if (ContentURIData.URIMember != null)
            {
                this.Member = ContentURIData.URIMember;
            }
            return bHasSet;
        }

        public async Task<string> UpdatePaymentHandling(HttpContext context, IMemberService memberService,
            AccountToMember loggedInMember)
        {
            string sError = Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                 string.Empty, "MEMBERVIEW_NOPAYS");
            //await memberService.UpdatePaymentHandlingAsync(loggedInMember);
            return sError;
        }
   
    }
}
