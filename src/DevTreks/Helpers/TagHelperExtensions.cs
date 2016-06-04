using DevTreks.Data.Helpers;
using DevTreks.Data;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Text;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.WebEncoders;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using DevTreks.Data.RuleHelpers;
using DevTreks.Data.EditHelpers;
using DevTreks.Data.AppHelpers;
using DevTreks.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.TagHelpers;

namespace DevTreks.Helpers.TagHelpers
{
    /// <summary>
    ///Purpose:		Class for helping razor pages render tags
    ///Author:		www.devtreks.org
    ///Date:		2016, April
    ///NOTES        not developed: appears more applicable to Razor pages rather than custom code
    /// </summary>
    public class EmailTagHelper : TagHelper
    {
        private const string EmailDomain = "contoso.com";
        // Can be passed via <email mail-to="..." />. 
        // Pascal case gets translated into lower-kebab-case. 
        public string MailTo { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a"; 
            // Replaces <email> with <a> tag
            var address = MailTo + "@" + EmailDomain;
            //output.Attributes["href"] = "mailto:" + address;
            output.Content.SetContent(address);
        }
    }
}
