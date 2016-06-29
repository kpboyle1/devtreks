using DevTreks.Models;
using DevTreks.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Encodings.Web;
using DevTreks.Data;
using DataAppHelpers = DevTreks.Data.AppHelpers;
using DataHelpers = DevTreks.Data.Helpers;
using RuleHelpers = DevTreks.Data.RuleHelpers;
using EditHelpers = DevTreks.Data.EditHelpers;

namespace DevTreks.Helpers
{
    /// <summary>
    ///Purpose:		static Html extensions for helping member views
    ///Author:		www.devtreks.org
    ///Date:		2016, April
    ///References:	www.devtreks.org
    /// </summary>
    public static class HtmlMemberExtensions
    {
        /// <summary>
        /// Determines whether the site recognizes the current user
        /// </summary>
        public static bool UserIsLoggedIn(HttpContext context)
        {
            bool result = false;
            if (context != null)
            {
                result = context.User.Identity.IsAuthenticated;
            }
            return result;
        }
        public static void SignOut(HttpContext context)
        {
            //Account Controller has sign out code
            // clear everything
            if (context.Request.Cookies.Count > 0)
            {
                //context fails in unit testing (mock needs more web mocks), so count condition added
                //Task t = context.Authentication.SignOutAsync();
                //context.Request.Cookies.Clear();
                //context.Response.Cookies.Clear();
                //HttpCookie accountCookie = new HttpCookie(FormsAuthentication.FormsCookieName, string.Empty);
                //context.Response.Cookies.Add(accountCookie);
                //new 
                context.User = null;
            }
        }
        
        public static HtmlString FillInRegions(this IHtmlHelper helper, ContentURI uri,
            string selectName, DataHelpers.GeneralHelpers.VIEW_EDIT_TYPES viewEditType,
            int geoRegionId)
        {
            using (StringWriter result = new StringWriter())
            {
                if (uri.URIDataManager.GeoRegions != null)
                {
                    string sCssClass = "Select250";
                    string sJavascriptMethod = (geoRegionId == 0) ? string.Empty : string.Empty;
                    bool bIsSelected = (geoRegionId == 0) ? true : false;
                    if (viewEditType != DataHelpers.GeneralHelpers.VIEW_EDIT_TYPES.full)
                    {
                        foreach (GeoRegion georegion in uri.URIDataManager.GeoRegions)
                        {
                            if (georegion.PKId == geoRegionId)
                            {
                                bIsSelected = true;
                            }
                            else
                            {
                                bIsSelected = false;
                            }
                            if (bIsSelected)
                            {
                                result.Write(georegion.GeoRegionName);
                                break;
                            }
                        }
                    }
                    else
                    {
                        result.WriteLine(helper.SelectStart(viewEditType, string.Concat("GeoRegionId", geoRegionId.ToString()),
                            selectName, sCssClass));
                        result.WriteLine(helper.Option(AppHelper.GetResource("MAKE_SELECTION"),
                            "0", bIsSelected));
                        bIsSelected = false;
                        foreach (GeoRegion georegion in uri.URIDataManager.GeoRegions)
                        {
                            if (georegion.PKId == geoRegionId)
                            {
                                bIsSelected = true;
                            }
                            else
                            {
                                bIsSelected = false;
                            }
                            result.WriteLine(helper.Option(georegion.GeoRegionName,
                                georegion.PKId.ToString(), bIsSelected));
                        }
                        result.WriteLine(helper.SelectEnd());
                    }
                }
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString FillInClubGroups(this IHtmlHelper helper, ContentURI uri,
            string selectName, DataHelpers.GeneralHelpers.VIEW_EDIT_TYPES viewEditType,
            int clubGroupId)
        {
            using (StringWriter result = new StringWriter())
            {
                if (uri.URIDataManager.ClubGroups != null)
                {
                    string sCssClass = "Select250";
                    string sJavascriptMethod = (clubGroupId == 0) ? string.Empty : string.Empty;
                    bool bIsSelected = (clubGroupId == 0) ? true : false;
                    if (viewEditType != DevTreks.Data.Helpers.GeneralHelpers.VIEW_EDIT_TYPES.full)
                    {
                        foreach (AccountClass clubgroup in uri.URIDataManager.ClubGroups)
                        {
                            if (clubgroup.PKId == clubGroupId)
                            {
                                bIsSelected = true;
                            }
                            else
                            {
                                bIsSelected = false;
                            }
                            if (bIsSelected)
                            {
                                result.Write(clubgroup.AccountClassName);
                                break;
                            }
                        }
                    }
                    else
                    {
                        result.WriteLine(helper.SelectStart(viewEditType, string.Concat("ClubGroupId", clubGroupId.ToString()),
                            selectName, sCssClass));
                        result.WriteLine(helper.Option(AppHelper.GetResource("MAKE_SELECTION"),
                            "0", bIsSelected));
                        bIsSelected = false;
                        foreach (AccountClass clubgroup in uri.URIDataManager.ClubGroups)
                        {
                            if (clubgroup.PKId == clubGroupId)
                            {
                                bIsSelected = true;
                            }
                            else
                            {
                                bIsSelected = false;
                            }
                            result.WriteLine(helper.Option(clubgroup.AccountClassName,
                                clubgroup.PKId.ToString(), bIsSelected));
                        }
                        result.WriteLine(helper.SelectEnd());
                    }
                }
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString FillInMemberGroups(this IHtmlHelper helper, ContentURI uri,
            string selectName, DataHelpers.GeneralHelpers.VIEW_EDIT_TYPES viewEditType,
            int memberGroupId)
        {
            using (StringWriter result = new StringWriter())
            {
                if (uri.URIDataManager.MemberGroups != null)
                {
                    string sCssClass = "Select250";
                    string sJavascriptMethod = (memberGroupId == 0) ? string.Empty : string.Empty;
                    bool bIsSelected = (memberGroupId == 0) ? true : false;
                    if (viewEditType != DevTreks.Data.Helpers.GeneralHelpers.VIEW_EDIT_TYPES.full)
                    {
                        foreach (MemberClass membergroup in uri.URIDataManager.MemberGroups)
                        {
                            if (membergroup.PKId == memberGroupId)
                            {
                                bIsSelected = true;
                            }
                            else
                            {
                                bIsSelected = false;
                            }
                            if (bIsSelected)
                            {
                                result.Write(membergroup.MemberClassName);
                                break;
                            }
                        }
                    }
                    else
                    {
                        result.WriteLine(helper.SelectStart(viewEditType, string.Concat("MemberGroupId", memberGroupId.ToString()),
                            selectName, sCssClass));
                        result.WriteLine(helper.Option(AppHelper.GetResource("MAKE_SELECTION"),
                            "0", bIsSelected));
                        bIsSelected = false;
                        foreach (MemberClass membergroup in uri.URIDataManager.MemberGroups)
                        {
                            if (membergroup.PKId == memberGroupId)
                            {
                                bIsSelected = true;
                            }
                            else
                            {
                                bIsSelected = false;
                            }
                            result.WriteLine(helper.Option(membergroup.MemberClassName,
                                membergroup.PKId.ToString(), bIsSelected));
                        }
                        result.WriteLine(helper.SelectEnd());
                    }
                }
                return new HtmlString(result.ToString());
            }
        }
        
        public static HtmlString WriteGeoRegionsSelectList(this IHtmlHelper helper,
            ContentURI uri, string selectListName,
            string viewEditType, string geoRegionId)
        {
            int iGeoRegionId = DataHelpers.GeneralHelpers.ConvertStringToInt(geoRegionId);
            DataHelpers.GeneralHelpers.VIEW_EDIT_TYPES eViewEditType = (!string.IsNullOrEmpty(viewEditType))
                ? (DataHelpers.GeneralHelpers.VIEW_EDIT_TYPES)Enum.Parse(typeof(DataHelpers.GeneralHelpers.VIEW_EDIT_TYPES), viewEditType)
                : DataHelpers.GeneralHelpers.VIEW_EDIT_TYPES.print;
            return helper.FillInRegions(uri, selectListName,
                eViewEditType, iGeoRegionId);
        }
        public static HtmlString WriteClubGroupsSelectList(this IHtmlHelper helper,
            ContentURI uri, string selectListName,
            string viewEditType, string accountGroupId)
        {
            int iClubGroupId = DataHelpers.GeneralHelpers.ConvertStringToInt(accountGroupId);
            DataHelpers.GeneralHelpers.VIEW_EDIT_TYPES eViewEditType = (!string.IsNullOrEmpty(viewEditType))
                ? (DataHelpers.GeneralHelpers.VIEW_EDIT_TYPES)Enum.Parse(typeof(DataHelpers.GeneralHelpers.VIEW_EDIT_TYPES), viewEditType)
                : DataHelpers.GeneralHelpers.VIEW_EDIT_TYPES.print;
            return helper.FillInClubGroups(uri, selectListName,
               eViewEditType, iClubGroupId);
        }
        public static HtmlString WriteMemberGroupsSelectList(this IHtmlHelper helper, 
            ContentURI uri, string selectListName, string viewEditType, string memberGroupId)
        {
            int iMemberGroupId = DataHelpers.GeneralHelpers.ConvertStringToInt(memberGroupId);
            DataHelpers.GeneralHelpers.VIEW_EDIT_TYPES eViewEditType = (!string.IsNullOrEmpty(viewEditType))
                ? (DataHelpers.GeneralHelpers.VIEW_EDIT_TYPES)Enum.Parse(typeof(DataHelpers.GeneralHelpers.VIEW_EDIT_TYPES), viewEditType)
                : DataHelpers.GeneralHelpers.VIEW_EDIT_TYPES.print;
            return helper.FillInMemberGroups(uri, selectListName,
                eViewEditType, iMemberGroupId);
        }
        

        public static void WriteDefaultClubPayments(this IHtmlHelper helper,
           ContentURI uri)
        {
            //2.0.0 no subscriptions yet via services
            helper.PStart();
            helper.Raw("Service subscriptions are not supported yet.");
            helper.PEnd();

            //HtmlUtils.StartStrong(writer);
            //writer.Write(AppHelper.GetResource("CLUB_PAYMENTS"));
            //HtmlUtils.FinishStrong(writer);
            //writer.Write("<br />");
            //writer.Write("<br />");
            //if (uri.URIMember.ClubDefault != null)
            //{
            //    if (uri.URIMember.ClubDefault.AccountToPayment != null)
            //    {
            //        if (uri.URIMember.ClubDefault.AccountToPayment.Count > 0)
            //        {
            //            HtmlUtils.StartTable(writer, "data", "5", "5");
            //            HtmlUtils.StartTableRow(writer, string.Empty, string.Empty);
            //            HtmlUtils.StartCell(writer, string.Empty, string.Empty, 1, "row");
            //            writer.Write("Service Id");
            //            HtmlUtils.FinishCell(writer);
            //            HtmlUtils.StartCell(writer, string.Empty, string.Empty, 1, string.Empty);
            //            writer.Write("Revenue Due");
            //            HtmlUtils.FinishCell(writer);
            //            HtmlUtils.StartCell(writer, string.Empty, string.Empty, 1, string.Empty);
            //            writer.Write("Revenue Due Date");
            //            HtmlUtils.FinishCell(writer);
            //            HtmlUtils.StartCell(writer, string.Empty, string.Empty, 1, "row");
            //            writer.Write("Revenue Paid");
            //            HtmlUtils.FinishCell(writer);
            //            HtmlUtils.StartCell(writer, string.Empty, string.Empty, 1, string.Empty);
            //            writer.Write("Revenue Paid Date");
            //            HtmlUtils.FinishCell(writer);
            //            HtmlUtils.StartCell(writer, string.Empty, string.Empty, 1, string.Empty);
            //            writer.Write("Payment Due");
            //            HtmlUtils.FinishCell(writer);
            //            HtmlUtils.StartCell(writer, string.Empty, string.Empty, 1, string.Empty);
            //            writer.Write("Payment Due Date");
            //            HtmlUtils.FinishCell(writer);
            //            HtmlUtils.StartCell(writer, string.Empty, string.Empty, 1, "row");
            //            writer.Write("Payment Paid");
            //            HtmlUtils.FinishCell(writer);
            //            HtmlUtils.StartCell(writer, string.Empty, string.Empty, 1, string.Empty);
            //            writer.Write("Payment Paid Date");
            //            HtmlUtils.FinishCell(writer);
            //            HtmlUtils.FinishRow(writer);
            //            {
            //                foreach (AccountToPayment pay in uri.URIMember.ClubDefault.AccountToPayment)
            //                {
            //                    HtmlUtils.StartTableRow(writer, string.Empty, string.Empty);
            //                    HtmlUtils.StartCell(writer, string.Empty, string.Empty, 1, "row");
            //                    writer.Write(pay.AccountToServiceId);
            //                    HtmlUtils.FinishCell(writer);
            //                    HtmlUtils.StartCell(writer, string.Empty, string.Empty, 1, string.Empty);
            //                    writer.Write(pay.CreditDue);
            //                    HtmlUtils.FinishCell(writer);
            //                    HtmlUtils.StartCell(writer, string.Empty, string.Empty, 1, string.Empty);
            //                    writer.Write(pay.CreditDueDate);
            //                    HtmlUtils.FinishCell(writer);
            //                    HtmlUtils.StartCell(writer, string.Empty, string.Empty, 1, "row");
            //                    writer.Write(pay.CreditPaid);
            //                    HtmlUtils.FinishCell(writer);
            //                    HtmlUtils.StartCell(writer, string.Empty, string.Empty, 1, string.Empty);
            //                    writer.Write(pay.CreditPaidDate);
            //                    HtmlUtils.FinishCell(writer);
            //                    HtmlUtils.FinishCell(writer);
            //                    HtmlUtils.StartCell(writer, string.Empty, string.Empty, 1, string.Empty);
            //                    writer.Write(pay.PaymentDue);
            //                    HtmlUtils.FinishCell(writer);
            //                    HtmlUtils.StartCell(writer, string.Empty, string.Empty, 1, string.Empty);
            //                    writer.Write(pay.PaymentDueDate);
            //                    HtmlUtils.FinishCell(writer);
            //                    HtmlUtils.StartCell(writer, string.Empty, string.Empty, 1, "row");
            //                    writer.Write(pay.PaymentPaid);
            //                    HtmlUtils.FinishCell(writer);
            //                    HtmlUtils.StartCell(writer, string.Empty, string.Empty, 1, string.Empty);
            //                    writer.Write(pay.PaymentPaidDate);
            //                    HtmlUtils.FinishCell(writer);
            //                    HtmlUtils.FinishRow(writer);
            //                }
            //            }
            //            HtmlUtils.FinishTable(writer);
            //        }
            //        else
            //        {
            //            uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
            //                string.Empty, "MEMBERVIEW_NOPAYS");
            //        }
            //    }
            //    else
            //    {
            //        uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
            //            string.Empty, "MEMBERVIEW_BADDEFAULTCLUB");
            //    }
            //}
            //else
            //{
            //    uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
            //            string.Empty, "MEMBERVIEW_BADDEFAULTCLUB");
            //}
            //writer.Write("<br />");
        }
        
        public static void FillInClubCreditFormData(Microsoft.AspNetCore.Http.HttpContext context,
           AccountToMember loggedInMember)
        {
            if (loggedInMember != null)
            {
                if (loggedInMember.ClubDefault == null)
                {
                    //retrieve the account id and build a new club
                    loggedInMember.ClubDefault = new Account();
                }
                if (loggedInMember.ClubDefault.AccountToCredit == null)
                {
                    loggedInMember.ClubDefault.AccountToCredit = new List<AccountToCredit>();
                    loggedInMember.ClubDefault.AccountToCredit.Add(new AccountToCredit());
                }
                if (loggedInMember.ClubDefault.AccountToCredit.Count == 0)
                {
                    loggedInMember.ClubDefault.AccountToCredit.Add(new AccountToCredit());
                }
                string sFormValue = context.Request.Form.FirstOrDefault(k => k.Key == "txtCardFullName").Value;
                //string sFormValue = context.Request.Form["txtCardFullName"] ?? AppHelper.GetResource("NONE");
                if (sFormValue.EndsWith("*"))
                {
                    //get rid of the javascript-inserted stars
                    sFormValue = sFormValue.Replace("*", string.Empty);
                }
                loggedInMember.ClubDefault.AccountToCredit.FirstOrDefault().CardFullName = sFormValue;
                //sFormValue = context.Request.Form["txtCardFullNumber"] ?? AppHelper.GetResource("NONE");
                if (sFormValue.EndsWith("*"))
                {
                    //get rid of the javascript-inserted stars
                    sFormValue = sFormValue.Replace("*", string.Empty);
                }
                loggedInMember.ClubDefault.AccountToCredit.FirstOrDefault().CardFullNumber = sFormValue;
                //sFormValue = context.Request.Form["txtCardShortNumber"] ?? AppHelper.GetResource("NONE");
                if (sFormValue.EndsWith("*"))
                {
                    //get rid of the javascript-inserted stars
                    sFormValue = sFormValue.Replace("*", string.Empty);
                }
                loggedInMember.ClubDefault.AccountToCredit.FirstOrDefault().CardShortNumber = sFormValue;
                //sFormValue = context.Request.Form["selectcardtype"] ?? AppHelper.GetResource("NONE");
                if (sFormValue.EndsWith("*"))
                {
                    //get rid of the javascript-inserted stars
                    sFormValue = sFormValue.Replace("*", string.Empty);
                }
                loggedInMember.ClubDefault.AccountToCredit.FirstOrDefault().CardType = sFormValue;
                //sFormValue = context.Request.Form["selectcardmonth"] ?? AppHelper.GetResource("NONE");
                if (sFormValue.EndsWith("*"))
                {
                    //get rid of the javascript-inserted stars
                    sFormValue = sFormValue.Replace("*", string.Empty);
                }
                loggedInMember.ClubDefault.AccountToCredit.FirstOrDefault().CardEndMonth = sFormValue;
                //sFormValue = context.Request.Form["selectcardyear"]  ?? AppHelper.GetResource("NONE");
                if (sFormValue.EndsWith("*"))
                {
                    //get rid of the javascript-inserted stars
                    sFormValue = sFormValue.Replace("*", string.Empty);
                }
                loggedInMember.ClubDefault.AccountToCredit.FirstOrDefault().CardEndYear = sFormValue;

                //why store state? is it a requirement?
                loggedInMember.ClubDefault.AccountToCredit.FirstOrDefault().CardState = AppHelper.GetResource("NONE");//context.Request.Form["selectcardlocation"] ?? AppHelper.GetResource("NONE");
            }
        }
    }
}
