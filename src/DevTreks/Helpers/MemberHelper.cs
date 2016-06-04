using DevTreks.Data;
using DevTreks.Models;
using DevTreks.Services;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GenHelpers = DevTreks.Data.Helpers.GeneralHelpers;

namespace DevTreks.Helpers
{
    /// <summary>
    ///Purpose:		methods for helping member views
    ///Author:		www.devtreks.org
    ///Date:		2016, April
    ///References:	www.devtreks.org
    /// </summary>
    public class MemberHelper
	{
        public MemberHelper() { }

        public async Task<bool> SetClubAndMemberAsync(HttpContext context, bool isInitView,
            IMemberService memberService, ContentURI uri)
        {
            //switch to memberService.SetDefaultClubandMember();
            bool bHasMember 
                = await memberService.SetDefaultClubAndMemberAsync(uri, context.User.Identity.Name);
            //increase performance by only showing this when members need to view club panel
            if (isInitView && HtmlMemberExtensions.UserIsLoggedIn(context)
                && uri.URIDataManager.ServerActionType == GenHelpers.SERVER_ACTION_TYPES.member)
            {
                //context relies on uri.URIMember.ClubDefault (not iaccountid)
                await memberService.SetMemberClubDefaultAsync(uri, isInitView, uri.URIMember.ClubDefault.PKId);
                //set last 5 edited uris
                uri.URIMember.ClubDefault.AccountToAudit
                    = await memberService.GetClubLast5EditedItemsAsync(
                        uri, uri.URIMember.ClubDefault.PKId);
            }
            //clubinuse inits with default club; but changes when a service is used
            //if no members.clubs have a subscription, clubinuse is anonymous
            uri.URIMember.ClubInUse = new Account(uri.URIMember.ClubDefault);
            return bHasMember;
        }
        
        private async Task<AccountToMember> GetFirstClubAsync(
            IMemberService memberService, ContentURI uri,
            Member existingBaseMember)
        {
            AccountToMember existingMember = new AccountToMember(true);
            if (existingBaseMember != null)
            {
                if (existingBaseMember.PKId != 0)
                {
                    existingMember.Member = new DevTreks.Models.Member(existingBaseMember);
                    //most ui navigation takes place using memberid property
                    existingMember.MemberId = existingBaseMember.PKId;
                }
                //doesn't have a default club, see if one can be made the default for now
                IList<AccountToMember> clubs 
                    = await memberService.GetClubsByMemberAsync(uri,
                        existingBaseMember.PKId);
                if (clubs != null)
                {
                    if (clubs.Count > 0)
                    {
                        existingMember = clubs.FirstOrDefault();
                    }
                }
            }
            return existingMember;
        }
        
        public async Task<ContentURI> MakeNewURIWithGoodContentAndMemberAsync(Microsoft.AspNetCore.Http.HttpContext context,
            IContentService contentService, IMemberService memberService,
            ContentURI uri, string newURIPattern)
        {
            ContentURI oNewURI = ContentURI.ConvertShortURIPattern(newURIPattern, uri.URINetwork);
            oNewURI.URIDataManager.FormInput = uri.URIDataManager.FormInput;
            //set uri props needed to get a good filepath
            //set apps, subapps and servicegroups
            GenHelpers.SetApps(oNewURI);
            //set misc. properties needed by subsequent methods
            oNewURI.URIDataManager.PageSize = uri.URIDataManager.PageSize;
            //set member and club
            bool bIsInitView = true;
            bool bHasSet = await SetClubAndMemberAsync(context, bIsInitView, memberService, oNewURI);
            //set the service for context app (or file paths will be to agreements not apps)
            bHasSet = await contentService.SetServiceAndChangeAppAsync(oNewURI, oNewURI.URIId);
            //set content model properties
            bHasSet = await contentService.SetContentModelAndAncestorsAsync(memberService, oNewURI, bIsInitView);
            return oNewURI;
        }
        
        public async Task<bool> AddGoodContentAndMemberToNewURIAsync(HttpContext context,
            IContentService contentService, IMemberService memberService,
            ContentURI uri)
        {
            //set uri props needed to get a good filepath
            //set apps, subapps and servicegroups
            GenHelpers.SetApps(uri);
            //set member and club
            bool bIsInitView = true;
            bool bHasSet = await SetClubAndMemberAsync(context, bIsInitView, memberService, uri);
            //set the service for context app (or file paths will be to agreements not apps)
            bHasSet = await contentService.SetServiceAndChangeAppAsync(uri, uri.URIId);
            //set content model properties
            bHasSet = await contentService.SetContentModelAndAncestorsAsync(memberService, uri, bIsInitView);
            return bHasSet;
        }
	}
}
