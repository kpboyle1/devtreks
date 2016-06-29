using DevTreks.Data.EditHelpers;
using DevTreks.Data.Helpers;
using DevTreks.Data;
using DevTreks.Helpers;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Encodings.Web;

namespace DevTreks.Helpers
{
    /// <summary>
    ///Purpose:		static utility methods for the presentation layer xhtml manipulation
    ///Author:		www.devtreks.org
    ///Date:		2016, June
    ///References:	www.devtreks.org
    /// </summary>
    /// Notes    
    /// EndTag	Represents the mode for rendering a closing tag (for example, </tag>).
    /// Normal Represents the mode for rendering normal text. (>?)
    /// SelfClosing Represents the mode for rendering a self-closing tag(for example, <tag />).
    /// StartTag Represents the mode for rendering an opening tag(for example, <tag>).
    public static class HtmlExtensions
    {
        public const string DISCARD_SELECTS = "discardselects";
        public const string CANCEL_EDITS = "btnCancelAllDoc";
        public const string GO_BACK = "goback";
        public const string GO_FORWARD = "goforward";
        public const string START_ROW = "startrow";
        public const string OLDSTART_ROW = "oldstartrow";
        public const string PARENTSTART_ROW = "parentstartrow";
        //html 5 attribute names for unobtrusive js
        public const string DATA_CP = "data-contenturipattern";
        public const string DATA_CA = "data-clientaction";
        public const string DATA_PARAMS = "data-params";
        public const string DATA_ID1 = "data-id1";
        public const string DATA_ATTNAME = "data-attname";
        public const string DATA_UP1 = "data-uripattern1";
        public const string DATA_NODENAME = "data-nodename";
        public const string DATA_UNITID = "data-unitid";
        public const string DATA_FORMPARAM = "data-formparam";
        public static void GetSearchStartAtRowButtonsClass(int startRow, int pageSize,
           int rowCount, out string class1, out string class2)
        {
            class1 = "SearchButton1Enabled";
            class2 = "SearchButton1Enabled";
            class1 = (startRow <= 0) ? "Button1Disabled" : "SearchButton1Enabled";
            if (rowCount <= (startRow + pageSize))
            {
                class2 = "Button1Disabled";
            }
            if (rowCount <= 0)
            {
                class1 = "Button1Disabled";
                class2 = "Button1Disabled";
            }
        }
        public static void GetStartAtRowButtonsClass(int startRow, int pageSize,
           int rowCount, out string class1, out string class2)
        {
            class1 = "Button1Enabled";
            class2 = "Button1Enabled";
            class1 = (startRow <= 0) ? "Button1Disabled" : "Button1Enabled";
            if (rowCount <= (startRow + pageSize))
            {
                class2 = "Button1Disabled";
            }
            if (rowCount <= 0)
            {
                class1 = "Button1Disabled";
                class2 = "Button1Disabled";
            }
        }
        //mvc6 helper methods
        public static HtmlString WriteTagBuilderHtml(TagBuilder tb)
        {
            using (StringWriter sw = new StringWriter())
            {
                HtmlEncoder enc = HtmlEncoder.Default;
                tb.WriteTo(sw, enc);
                return new HtmlString(sw.ToString());
            }
        }
        private static string ConvertHtmlString(HtmlString html)
        {
            using (StringWriter sw = new StringWriter())
            {
                HtmlEncoder enc = HtmlEncoder.Default;
                html.WriteTo(sw, enc);
                return sw.ToString();
            }
        }
        private static string WriteTagBuilderString(TagBuilder tb)
        {
            using (StringWriter sw = new StringWriter())
            {
                HtmlEncoder enc = HtmlEncoder.Default;
                tb.WriteTo(sw, enc);
                return sw.ToString();
            }
        }
        private static string WriteTagBuilderString(TagBuilder tb,
            StringWriter sw)
        {
            //assumes that the calling method has 
            //using StringWriter sw = new StringWriter(){}
            HtmlEncoder enc = HtmlEncoder.Default;
            tb.WriteTo(sw, enc);
            return sw.ToString();
        }
        
        
        public static HtmlString PStart(this IHtmlHelper helper)
        {
            return PStart();
        }
        public static HtmlString PStart()
        {
            var tagBuilder = new TagBuilder("p");
            tagBuilder.TagRenderMode = TagRenderMode.StartTag;
            return (WriteTagBuilderHtml(tagBuilder));
        }
        public static HtmlString PEnd(this IHtmlHelper helper)
        {
            return (PEnd());
        }
        public static HtmlString PEnd()
        {
            var tagBuilder = new TagBuilder("p");
            tagBuilder.TagRenderMode = TagRenderMode.EndTag;
            return (WriteTagBuilderHtml(tagBuilder));
        }
        public static HtmlString ULStart(this IHtmlHelper helper, 
            string id, string classAtt)
        {
            return ULStart(id, classAtt);
        }
        public static HtmlString ULStart(string id, string classAtt)
        {
            var tagBuilder = new TagBuilder("ul");
            if (id != string.Empty)
            {
                tagBuilder.MergeAttribute("id", id);
            }
            if (classAtt != string.Empty)
            {
                tagBuilder.MergeAttribute("class", classAtt);
            }
            tagBuilder.TagRenderMode = TagRenderMode.StartTag;
            return (WriteTagBuilderHtml(tagBuilder));
        }
        public static HtmlString ULStart(this IHtmlHelper helper,
            string id, string classAtt, string dataRole,
            string dataInset, string dataTheme)
        {
            return (ULStart(id, classAtt, dataRole,
            dataInset, dataTheme));
        }
        public static HtmlString ULStart(
            string id, string classAtt, string dataRole,
            string dataInset, string dataTheme)
        {
            var tagBuilder = new TagBuilder("ul");
            if (id != string.Empty)
            {
                tagBuilder.MergeAttribute("id", id);
            }
            if (classAtt != string.Empty)
            {
                tagBuilder.MergeAttribute("class", classAtt);
            }
            if (dataRole != string.Empty)
            {
                tagBuilder.MergeAttribute("data-role", dataRole);
            }
            if (dataInset != string.Empty)
            {
                tagBuilder.MergeAttribute("data-inset", dataInset);
            }
            if (dataTheme != string.Empty)
            {
                tagBuilder.MergeAttribute("data-theme", dataTheme);
            }
            tagBuilder.TagRenderMode = TagRenderMode.StartTag;
            return (WriteTagBuilderHtml(tagBuilder));
        }
        public static HtmlString ULEnd(this IHtmlHelper helper)
        {
            return (ULEnd());
        }
        public static HtmlString ULEnd()
        {
            var tagBuilder = new TagBuilder("ul");
            tagBuilder.TagRenderMode = TagRenderMode.EndTag;
            return (WriteTagBuilderHtml(tagBuilder));
        }
        public static HtmlString LIStart(this IHtmlHelper helper,
            string dataRole)
        {
            return (LIStart(dataRole));
        }
        public static HtmlString LIStart(string dataRole)
        {
            var tagBuilder = new TagBuilder("li");
            if (dataRole != string.Empty)
            {
                tagBuilder.MergeAttribute("data-role", dataRole);
            }
            tagBuilder.TagRenderMode = TagRenderMode.StartTag;
            return (WriteTagBuilderHtml(tagBuilder));
        }
        public static HtmlString ULStart(this IHtmlHelper helper,
            string dataRole, string dataTheme, string dataDividerTheme)
        {
            return ULStart(dataRole, dataTheme, dataDividerTheme);
        }
        public static HtmlString ULStart(string dataRole,
            string dataTheme, string dataDividerTheme)
        {
            var tagBuilder = new TagBuilder("ul");
            if (dataRole != string.Empty)
            {
                tagBuilder.MergeAttribute("data-role", dataRole);
            }
            if (dataRole != string.Empty)
            {
                tagBuilder.MergeAttribute("data-theme", dataTheme);
            }
            if (dataRole != string.Empty)
            {
                tagBuilder.MergeAttribute("data-divider-theme", dataDividerTheme);
            }
            tagBuilder.TagRenderMode = TagRenderMode.StartTag;
            return (WriteTagBuilderHtml(tagBuilder));
        }
        public static HtmlString LIStart(this IHtmlHelper helper,
            string id, string classAtt)
        {
            return (LIStart(id, classAtt));
        }
        public static HtmlString LIStart(string id, string classAtt)
        {
            var tagBuilder = new TagBuilder("li");
            if (id != string.Empty)
            {
                tagBuilder.MergeAttribute("id", id);
            }
            if (classAtt != string.Empty)
            {
                tagBuilder.MergeAttribute("class", classAtt);
            }
            tagBuilder.TagRenderMode = TagRenderMode.StartTag;
            return (WriteTagBuilderHtml(tagBuilder));
        }
        public static HtmlString LIEnd(this IHtmlHelper helper)
        {
            return (LIEnd());
        }
        public static HtmlString LIEnd()
        {
            var tagBuilder = new TagBuilder("li");
            tagBuilder.TagRenderMode = TagRenderMode.EndTag;
            return (WriteTagBuilderHtml(tagBuilder));
        }
        public static HtmlString Image(this IHtmlHelper helper,
            string id, string src, string altText)
        {
            return (Image(id, src, altText));
        }
        public static HtmlString Image(string id, string src, string altText)
        {
            var tagBuilder = new TagBuilder("img");
            tagBuilder.MergeAttribute("id", id);
            tagBuilder.MergeAttribute("src", src);
            tagBuilder.MergeAttribute("alt", altText);
            tagBuilder.TagRenderMode = TagRenderMode.SelfClosing;
            return (WriteTagBuilderHtml(tagBuilder));
        }
        public static HtmlString Image(this IHtmlHelper helper,
            string id, string src, string altText, string height,
            string width, string onClick)
        {
            return (Image(id, src, altText, height,
                width, onClick));
        }
        public static HtmlString Image(
            string id, string src, string altText, string height,
            string width, string onClick)
        {
            var tagBuilder = new TagBuilder("img");
            tagBuilder.MergeAttribute("id", id);
            tagBuilder.MergeAttribute("src", src);
            tagBuilder.MergeAttribute("itemprop", "contentUrl");
            if (altText != string.Empty)
            {
                tagBuilder.MergeAttribute("alt", altText);
            }
            if (onClick != string.Empty)
            {
                tagBuilder.MergeAttribute("onclick", onClick);
            }
            tagBuilder.MergeAttribute("height", height);
            tagBuilder.MergeAttribute("width", width);
            tagBuilder.MergeAttribute("border", "0");
            tagBuilder.TagRenderMode = TagRenderMode.SelfClosing;
            return (WriteTagBuilderHtml(tagBuilder));
        }

        
        public static HtmlString Link(this IHtmlHelper helper,
            string id, string href, string classAttribute,
            string text, string onclick = "")
        {
            return (Link(id, href, classAttribute,
                text, onclick));
        }
        public static HtmlString Link(
            string id, string href, string classAttribute,
            string text, string onclick = "")
        {
            var tagBuilder = new TagBuilder("a");
            tagBuilder.MergeAttribute("id", id);
            tagBuilder.MergeAttribute("href", href);
            tagBuilder.MergeAttribute("class", classAttribute);
            if (!string.IsNullOrEmpty(onclick))
            {
                tagBuilder.MergeAttribute("onclick", onclick);
            }
            //tagBuilder.InnerHtml.AppendHtml("<strong>Whats New</strong>");
            tagBuilder.InnerHtml.AppendHtml(text);
            tagBuilder.TagRenderMode = TagRenderMode.Normal;
            return (WriteTagBuilderHtml(tagBuilder));
        }
        public static HtmlString LinkMobile(this IHtmlHelper helper,
            string id, string href, string classAttribute,
            string text, string dataRole, string dataMini, string dataInline,
            string dataIcon, string dataIconPos, string onclick = "")
        {
            return (LinkMobile(id, href, classAttribute, 
                text, dataRole, dataMini, dataInline,
                dataIcon, dataIconPos, onclick));
        }
        public static HtmlString LinkMobile(
            string id, string href, string classAttribute,
            string text, string dataRole, string dataMini, string dataInline,
            string dataIcon, string dataIconPos, string onclick = "")
        {
            var tagBuilder = new TagBuilder("a");
            tagBuilder.MergeAttribute("id", id);
            tagBuilder.MergeAttribute("href", href);
            if (!string.IsNullOrEmpty(classAttribute))
                tagBuilder.MergeAttribute("class", classAttribute);
            if (!string.IsNullOrEmpty(onclick))
            {
                tagBuilder.MergeAttribute("onclick", onclick);
            }
            if (dataRole != string.Empty)
            {
                tagBuilder.MergeAttribute("data-role", dataRole);
            }
            if (dataMini != string.Empty)
            {
                tagBuilder.MergeAttribute("data-mini", dataMini);
            }
            if (dataInline != string.Empty)
            {
                tagBuilder.MergeAttribute("data-inline", dataInline);
            }
            if (dataIcon != string.Empty)
            {
                tagBuilder.MergeAttribute("data-icon", dataIcon);
            }
            if (dataIconPos != string.Empty)
            {
                tagBuilder.MergeAttribute("data-iconpos", dataIconPos);
            }
            tagBuilder.InnerHtml.AppendHtml(text);
            tagBuilder.TagRenderMode = TagRenderMode.Normal;
            return (WriteTagBuilderHtml(tagBuilder));
        }
        public static HtmlString LinkUnobtrusive(this IHtmlHelper helper,
            string id, string href, string classAttribute,
            string text, string contenturipattern, string clientaction,
            string extraParams)
        {
            return (LinkUnobtrusive(id, href, classAttribute,
                text, contenturipattern, clientaction,
                extraParams));
        }
        public static HtmlString LinkUnobtrusive(
            string id, string href, string classAttribute, 
            string text, string contenturipattern, string clientaction, 
            string extraParams)
        {
            var tagBuilder = new TagBuilder("a");
            tagBuilder.MergeAttribute("id", id);
            tagBuilder.MergeAttribute("href", href);
            tagBuilder.MergeAttribute("class", classAttribute);
            tagBuilder.MergeAttribute(DATA_CP, contenturipattern);
            tagBuilder.MergeAttribute(DATA_CA, clientaction);
            tagBuilder.MergeAttribute(DATA_PARAMS, extraParams);
            tagBuilder.InnerHtml.AppendHtml(text);
            tagBuilder.TagRenderMode = TagRenderMode.Normal;
            return (WriteTagBuilderHtml(tagBuilder));
        }
        public static HtmlString LinkUnobtrusiveMobile(this IHtmlHelper helper,
            string id, string href, string classAttribute,
            string text, string contenturipattern, string clientaction,
            string extraParams, string dataRole, string dataMini, string dataInline,
            string dataIcon, string dataIconPos)
        {
            return (LinkUnobtrusiveMobile(
                id, href, classAttribute,
                text, contenturipattern, clientaction,
                extraParams, dataRole, dataMini, dataInline,
                dataIcon, dataIconPos));
        }
        public static HtmlString LinkUnobtrusiveMobile(
            string id, string href, string classAttribute,
            string text, string contenturipattern, string clientaction,
            string extraParams, string dataRole, string dataMini, string dataInline,
            string dataIcon, string dataIconPos)
        {
            var tagBuilder = new TagBuilder("a");
            tagBuilder.MergeAttribute("id", id);
            tagBuilder.MergeAttribute("href", href);
            tagBuilder.MergeAttribute("class", classAttribute);
            tagBuilder.MergeAttribute(DATA_CP, contenturipattern);
            tagBuilder.MergeAttribute(DATA_CA, clientaction);
            tagBuilder.MergeAttribute(DATA_PARAMS, extraParams);
            if (dataRole != string.Empty)
            {
                tagBuilder.MergeAttribute("data-role", dataRole);
            }
            if (dataMini != string.Empty)
            {
                tagBuilder.MergeAttribute("data-mini", dataMini);
            }
            if (dataInline != string.Empty)
            {
                tagBuilder.MergeAttribute("data-inline", dataInline);
            }
            if (dataIcon != string.Empty)
            {
                tagBuilder.MergeAttribute("data-icon", dataIcon);
            }
            if (dataIconPos != string.Empty)
            {
                tagBuilder.MergeAttribute("data-iconpos", dataIconPos);
            }
            tagBuilder.InnerHtml.AppendHtml(text);
            tagBuilder.TagRenderMode = TagRenderMode.Normal;
            return (WriteTagBuilderHtml(tagBuilder));
        }
        public static HtmlString LinkPlusUnobtrusiveMobile(this IHtmlHelper helper,
            string id, string href, string classAttribute, string text,
            string spanId, string contenturipattern, string nodeURIPattern,
            string nodeName, string attributeName, string extraParams,
            string dataRole, string dataMini, string dataInline,
            string dataIcon, string dataIconPos)
        {
            return (LinkPlusUnobtrusiveMobile(
                id, href, classAttribute, text,
                spanId, contenturipattern, nodeURIPattern,
                nodeName, attributeName, extraParams,
                dataRole, dataMini, dataInline,
                dataIcon, dataIconPos));
        }
        public static HtmlString LinkPlusUnobtrusiveMobile(
            string id, string href, string classAttribute, string text,
            string spanId, string contenturipattern, string nodeURIPattern,
            string nodeName, string attributeName, string extraParams,
            string dataRole, string dataMini, string dataInline,
            string dataIcon, string dataIconPos)
        {
            var tagBuilder = new TagBuilder("a");
            tagBuilder.MergeAttribute("id", id);
            tagBuilder.MergeAttribute("href", href);
            tagBuilder.MergeAttribute("class", classAttribute);
            tagBuilder.MergeAttribute(DATA_ID1, spanId);
            tagBuilder.MergeAttribute(DATA_CP, contenturipattern);
            tagBuilder.MergeAttribute(DATA_UP1, nodeURIPattern);
            tagBuilder.MergeAttribute(DATA_NODENAME, nodeName);
            tagBuilder.MergeAttribute(DATA_ATTNAME, attributeName);
            tagBuilder.MergeAttribute(DATA_PARAMS, extraParams);
            if (dataRole != string.Empty)
            {
                tagBuilder.MergeAttribute("data-role", dataRole);
            }
            if (dataMini != string.Empty)
            {
                tagBuilder.MergeAttribute("data-mini", dataMini);
            }
            if (dataInline != string.Empty)
            {
                tagBuilder.MergeAttribute("data-inline", dataInline);
            }
            if (dataIcon != string.Empty)
            {
                tagBuilder.MergeAttribute("data-icon", dataIcon);
            }
            if (dataIconPos != string.Empty)
            {
                tagBuilder.MergeAttribute("data-iconpos", dataIconPos);
            }
            tagBuilder.InnerHtml.AppendHtml(text);
            tagBuilder.TagRenderMode = TagRenderMode.Normal;
            return (WriteTagBuilderHtml(tagBuilder));
        }
        
        public static HtmlString InputUnobtrusiveMobile(this IHtmlHelper helper,
            string id, string classAttribute, string type,
            string data_id1, string value, string dataMini, string dataInline,
            string dataIcon, string dataIconPos)
        {
            return (InputUnobtrusiveMobile(id, classAttribute, type,
                data_id1, value, dataMini, dataInline,
                dataIcon, dataIconPos));
        }
        public static HtmlString InputUnobtrusiveMobile(
            string id, string classAttribute, string type,
            string data_id1, string value, string dataMini, string dataInline,
            string dataIcon, string dataIconPos)
        {
            var tagBuilder = new TagBuilder("input");
            tagBuilder.MergeAttribute("id", id);
            tagBuilder.MergeAttribute("name", id);
            tagBuilder.MergeAttribute("class", classAttribute);
            tagBuilder.MergeAttribute("type", type);
            tagBuilder.MergeAttribute(DATA_ID1, data_id1);
            if (dataMini != string.Empty)
            {
                tagBuilder.MergeAttribute("data-mini", dataMini);
            }
            if (dataInline != string.Empty)
            {
                tagBuilder.MergeAttribute("data-inline", dataInline);
            }
            if (dataIcon != string.Empty)
            {
                tagBuilder.MergeAttribute("data-icon", dataIcon);
            }
            if (dataIconPos != string.Empty)
            {
                tagBuilder.MergeAttribute("data-iconpos", dataIconPos);
            }
            tagBuilder.MergeAttribute("value", value);
            tagBuilder.TagRenderMode = TagRenderMode.SelfClosing;
            return (WriteTagBuilderHtml(tagBuilder));
        }
        
        public static HtmlString InputUnobtrusiveMobile(this IHtmlHelper helper,
            string id, string classAttribute, string type,
            string contenturipattern, string clientaction,
            string extraParams, string value, string dataMini, string dataInline,
            string dataIcon, string dataIconPos)
        {
            return (InputUnobtrusiveMobile(
                id, classAttribute, type,
                contenturipattern, clientaction,
                extraParams, value, dataMini, dataInline,
                dataIcon, dataIconPos));
        }
        public static HtmlString InputUnobtrusiveMobile(
            string id, string classAttribute, string type,
            string contenturipattern, string clientaction,
            string extraParams, string value, string dataMini, string dataInline,
            string dataIcon, string dataIconPos)
        {
            var tagBuilder = new TagBuilder("input");
            tagBuilder.MergeAttribute("id", id);
            tagBuilder.MergeAttribute("class", classAttribute);
            tagBuilder.MergeAttribute("type", type);
            tagBuilder.MergeAttribute(DATA_CP, contenturipattern);
            tagBuilder.MergeAttribute(DATA_CA, clientaction);
            tagBuilder.MergeAttribute(DATA_PARAMS, extraParams);
            if (dataMini != string.Empty)
            {
                tagBuilder.MergeAttribute("data-mini", dataMini);
            }
            if (dataInline != string.Empty)
            {
                tagBuilder.MergeAttribute("data-inline", dataInline);
            }
            if (dataIcon != string.Empty)
            {
                tagBuilder.MergeAttribute("data-icon", dataIcon);
            }
            if (dataIconPos != string.Empty)
            {
                tagBuilder.MergeAttribute("data-iconpos", dataIconPos);
            }
            tagBuilder.MergeAttribute("value", value);
            tagBuilder.TagRenderMode = TagRenderMode.SelfClosing;
            return (WriteTagBuilderHtml(tagBuilder));
        }
        public static HtmlString InputPlusUnobtrusiveMobile(this IHtmlHelper helper,
            string classAttribute, string type, string id, string text,
            string spanId, string contenturipattern, string nodeURIPattern,
            string nodeName, string attributeName, string extraParams,
            string dataMini, string dataInline, string dataIcon, string dataIconPos)
        {
            return (InputPlusUnobtrusiveMobile(
                classAttribute, type, id, text,
                spanId, contenturipattern, nodeURIPattern,
                nodeName, attributeName, extraParams,
                dataMini, dataInline, dataIcon, dataIconPos));
        }
        public static HtmlString InputPlusUnobtrusiveMobile(
            string classAttribute, string type, string id, string text,
            string spanId, string contenturipattern, string nodeURIPattern,
            string nodeName, string attributeName, string extraParams,
            string dataMini, string dataInline, string dataIcon, string dataIconPos)
        {
            var tagBuilder = new TagBuilder("input");
            tagBuilder.MergeAttribute("id", id);
            tagBuilder.MergeAttribute("type", type);
            tagBuilder.MergeAttribute("class", classAttribute);
            tagBuilder.MergeAttribute(DATA_ID1, spanId);
            tagBuilder.MergeAttribute(DATA_CP, contenturipattern);
            tagBuilder.MergeAttribute(DATA_UP1, nodeURIPattern);
            tagBuilder.MergeAttribute(DATA_NODENAME, nodeName);
            tagBuilder.MergeAttribute(DATA_ATTNAME, attributeName);
            tagBuilder.MergeAttribute(DATA_PARAMS, extraParams);
            if (dataMini != string.Empty)
            {
                tagBuilder.MergeAttribute("data-mini", dataMini);
            }
            if (dataInline != string.Empty)
            {
                tagBuilder.MergeAttribute("data-inline", dataInline);
            }
            if (dataIcon != string.Empty)
            {
                tagBuilder.MergeAttribute("data-icon", dataIcon);
            }
            if (dataIconPos != string.Empty)
            {
                tagBuilder.MergeAttribute("data-iconpos", dataIconPos);
            }
            tagBuilder.MergeAttribute("value", text);
            tagBuilder.TagRenderMode = TagRenderMode.SelfClosing;
            //tagBuilder.InnerHtml.AppendHtml(text);
            //tagBuilder.TagRenderMode = TagRenderMode.Normal;
            return (WriteTagBuilderHtml(tagBuilder));
        }
        public static HtmlString Input(this IHtmlHelper helper,
            GeneralHelpers.VIEW_EDIT_TYPES editViewType, string id,
            string classAttribute, string type,
            string onclick, string name, string value)
        {
            return (Input(editViewType, id,
                classAttribute, type,
                onclick, name, value));
        }
        public static HtmlString Input(
            GeneralHelpers.VIEW_EDIT_TYPES editViewType, string id, 
            string classAttribute, string type, 
            string onclick, string name, string value)
        {
            if (editViewType != GeneralHelpers.VIEW_EDIT_TYPES.full
               && type != "hidden" && type != "button" && type != "reset")
            {
                return new HtmlString(value);
                //return MvcHtmlString.Create(value);
            }
            else
            {
                var tagBuilder = new TagBuilder("input");
                tagBuilder.MergeAttribute("id", id);
                if (!string.IsNullOrEmpty(classAttribute))
                {
                    tagBuilder.MergeAttribute("class", classAttribute);
                }
                tagBuilder.MergeAttribute("type", type);
                if (!string.IsNullOrEmpty(onclick))
                {
                    tagBuilder.MergeAttribute("onclick", onclick);
                }
                tagBuilder.MergeAttribute("name", name);
                tagBuilder.MergeAttribute("value", value);
                tagBuilder.MergeAttribute("data-mini", "true");
                tagBuilder.TagRenderMode = TagRenderMode.SelfClosing;
                return (WriteTagBuilderHtml(tagBuilder));
            }
        }
        public static HtmlString InputCheckBox(this IHtmlHelper helper,
            GeneralHelpers.VIEW_EDIT_TYPES editViewType, string id,
            string classAttribute, string type,
            string name, string value, bool isChecked)
        {
            return (InputCheckBox(editViewType, id,
                classAttribute, type,
                name, value, isChecked));
        }
        public static HtmlString InputCheckBox(
            GeneralHelpers.VIEW_EDIT_TYPES editViewType, string id,
            string classAttribute, string type,
            string name, string value, bool isChecked)
        {
            if (editViewType != GeneralHelpers.VIEW_EDIT_TYPES.full
               && type != "hidden" && type != "button" && type != "reset")
            {
                return new HtmlString(value);
                //return MvcHtmlString.Create(value);
            }
            else
            {
                var tagBuilder = new TagBuilder("input");
                tagBuilder.MergeAttribute("id", id);
                if (!string.IsNullOrEmpty(classAttribute))
                {
                    tagBuilder.MergeAttribute("class", classAttribute);
                }
                tagBuilder.MergeAttribute("type", type);
                if (isChecked)
                {
                    tagBuilder.MergeAttribute("checked", "checked");
                }
                tagBuilder.MergeAttribute("name", name);
                tagBuilder.MergeAttribute("value", value);
                tagBuilder.MergeAttribute("data-mini", "true");
                tagBuilder.TagRenderMode = TagRenderMode.SelfClosing;
                return (WriteTagBuilderHtml(tagBuilder));
            }
        }
        public static HtmlString InputMobile(this IHtmlHelper helper,
            GeneralHelpers.VIEW_EDIT_TYPES editViewType, string id,
            string classAttribute, string type,
            string onclick, string name, string value,
            string dataMini, string dataInline,
            string dataIcon, string dataIconPos)
        {
            return (InputMobile(editViewType, id,
                classAttribute, type, onclick, name, value,
                dataMini, dataInline, dataIcon, dataIconPos));
        }
        public static HtmlString InputMobile(
            GeneralHelpers.VIEW_EDIT_TYPES editViewType, string id,
            string classAttribute, string type,
            string onclick, string name, string value,
            string dataMini, string dataInline,
            string dataIcon, string dataIconPos)
        {
            if (editViewType != GeneralHelpers.VIEW_EDIT_TYPES.full
               && type != "hidden" && type != "button" && type != "reset")
            {
                return new HtmlString(value);
            }
            else
            {
                var tagBuilder = new TagBuilder("input");
                tagBuilder.MergeAttribute("id", id);
                tagBuilder.MergeAttribute("class", classAttribute);
                tagBuilder.MergeAttribute("type", type);
                if (!string.IsNullOrEmpty(onclick))
                {
                    tagBuilder.MergeAttribute("onclick", onclick);
                }
                tagBuilder.MergeAttribute("name", name);
                if (dataMini != string.Empty)
                {
                    tagBuilder.MergeAttribute("data-mini", dataMini);
                }
                if (dataInline != string.Empty)
                {
                    tagBuilder.MergeAttribute("data-inline", dataInline);
                }
                if (dataIcon != string.Empty)
                {
                    tagBuilder.MergeAttribute("data-icon", dataIcon);
                }
                if (dataIconPos != string.Empty)
                {
                    tagBuilder.MergeAttribute("data-iconpos", dataIconPos);
                }
                tagBuilder.MergeAttribute("value", value);
                tagBuilder.TagRenderMode = TagRenderMode.SelfClosing;
                return (WriteTagBuilderHtml(tagBuilder));
            }
        }
        public static HtmlString SelectUpdateStart(this IHtmlHelper helper,
            GeneralHelpers.VIEW_EDIT_TYPES editViewType, string uriPattern,
            string value, string propertyName, string dataType, string length,
            string classAttribute, string onclick)
        {
            return (SelectUpdateStart(
                editViewType, uriPattern,
                value, propertyName, dataType, length,
                classAttribute, onclick));
        }
        public static HtmlString SelectUpdateStart(
            GeneralHelpers.VIEW_EDIT_TYPES editViewType, string uriPattern,
            string value, string propertyName, string dataType, string length,
            string classAttribute, string onclick)
        {
            string sHtmlName = EditHelper.MakeStandardEditName(uriPattern, propertyName, dataType, length);
            var tagBuilder = new TagBuilder("select");
            tagBuilder.MergeAttribute("id", propertyName);
            if (editViewType == GeneralHelpers.VIEW_EDIT_TYPES.full)
            {
                tagBuilder.MergeAttribute("name", sHtmlName);
            }
            tagBuilder.MergeAttribute("class", classAttribute);
            tagBuilder.MergeAttribute("data-mini", "true");
            if (!string.IsNullOrEmpty(onclick))
            {
                tagBuilder.MergeAttribute("onclick", onclick);
            }
            tagBuilder.TagRenderMode = TagRenderMode.StartTag;
            return (WriteTagBuilderHtml(tagBuilder));
        }
        public static HtmlString SelectUnitStart(this IHtmlHelper helper,
            GeneralHelpers.VIEW_EDIT_TYPES editViewType, string uriPattern,
            string value, string propertyName, string dataType, string length,
            string classAttribute, string unitGroupId, string onclick)
        {
            return (SelectUnitStart(editViewType, uriPattern,
                value, propertyName, dataType, length,
                classAttribute, unitGroupId, onclick));
        }
        public static HtmlString SelectUnitStart(
            GeneralHelpers.VIEW_EDIT_TYPES editViewType, string uriPattern,
            string value, string propertyName, string dataType, string length,
            string classAttribute, string unitGroupId, string onclick)
        {
            string sHtmlName = EditHelper.MakeStandardEditName(uriPattern, propertyName, dataType, length);
            var tagBuilder = new TagBuilder("select");
            tagBuilder.MergeAttribute("id", propertyName);
            if (editViewType == GeneralHelpers.VIEW_EDIT_TYPES.full)
            {
                tagBuilder.MergeAttribute("name", sHtmlName);
            }
            tagBuilder.MergeAttribute("data-unitid", unitGroupId);
            tagBuilder.MergeAttribute("class", classAttribute);
            tagBuilder.MergeAttribute("data-mini", "true");
            if (!string.IsNullOrEmpty(onclick))
            {
                tagBuilder.MergeAttribute("onclick", onclick);
            }
            tagBuilder.TagRenderMode = TagRenderMode.StartTag;
            return (WriteTagBuilderHtml(tagBuilder));
        }
        public static HtmlString SelectStart(this IHtmlHelper helper,
            GeneralHelpers.VIEW_EDIT_TYPES editViewType, string id,
            string name, string classAttribute)
        {
            return (SelectStart(editViewType, id,
                name, classAttribute));
        }
        public static HtmlString SelectStart(
            GeneralHelpers.VIEW_EDIT_TYPES editViewType, string id,
            string name, string classAttribute)
        {
            var tagBuilder = new TagBuilder("select");
            tagBuilder.MergeAttribute("id", id);
            if (editViewType == GeneralHelpers.VIEW_EDIT_TYPES.full)
            {
                tagBuilder.MergeAttribute("name", name);
            }
            tagBuilder.MergeAttribute("class", classAttribute);
            tagBuilder.MergeAttribute("data-mini", "true");
            tagBuilder.TagRenderMode = TagRenderMode.StartTag;
            return (WriteTagBuilderHtml(tagBuilder));
        }
        public static HtmlString SelectEnd(this IHtmlHelper helper)
        {
            return SelectEnd();
        }
        public static HtmlString SelectEnd()
        {
            return new HtmlString("</select>");
        }
        public static HtmlString InputTextUpdate(this IHtmlHelper helper,
            GeneralHelpers.VIEW_EDIT_TYPES editViewType, string uriPattern,
            string value, string propertyName, string dataType, string length,
            string classAttribute, string onclick)
        {
            return InputTextUpdate(editViewType, uriPattern,
                value, propertyName, dataType, length,
                classAttribute, onclick);
        }
        public static HtmlString InputTextUpdate(
            GeneralHelpers.VIEW_EDIT_TYPES editViewType, string uriPattern,
            string value, string propertyNameForId, string dataType, string length,
            string classAttribute, string onclick)
        {
            var tagBuilder = new TagBuilder("input");
            tagBuilder.MergeAttribute("id", propertyNameForId);
            if (!string.IsNullOrEmpty(classAttribute))
            {
                tagBuilder.MergeAttribute("class", classAttribute);
            }
            tagBuilder.MergeAttribute("type", "text");
            if (editViewType != GeneralHelpers.VIEW_EDIT_TYPES.full)
            {
                tagBuilder.MergeAttribute("disabled", string.Empty);
            }
            else
            {
                string sHtmlName = EditHelper.MakeStandardEditName(uriPattern, 
                    propertyNameForId, dataType, length);
                tagBuilder.MergeAttribute("name", sHtmlName);
                if (!string.IsNullOrEmpty(onclick))
                {
                    tagBuilder.MergeAttribute("onclick", onclick);
                }
            }
            tagBuilder.MergeAttribute("data-mini", "true");
            tagBuilder.MergeAttribute("value", value);
            return (WriteTagBuilderHtml(tagBuilder));
        }
        public static HtmlString Input(this IHtmlHelper helper,
            string id, string classAttribute, string type,
            string onclick, string name, string value)
        {
            return Input(id, classAttribute, type,
                onclick, name, value);
        }
        public static HtmlString Input(
            string id, string classAttribute, string type,
            string onclick, string name, string value)
        {
            var tagBuilder = new TagBuilder("input");
            tagBuilder.MergeAttribute("id", id);
            if (!string.IsNullOrEmpty(classAttribute))
            {
                tagBuilder.MergeAttribute("class", classAttribute);
            }
            tagBuilder.MergeAttribute("type", type);
            if (!string.IsNullOrEmpty(onclick))
            {
                tagBuilder.MergeAttribute("onclick", onclick);
            }
            tagBuilder.MergeAttribute("name", name);
            tagBuilder.MergeAttribute("data-mini", "true");
            tagBuilder.MergeAttribute("value", value);
            return (WriteTagBuilderHtml(tagBuilder));
        }
        public static HtmlString IFrameStart(this IHtmlHelper helper,
            string id, string name, string classAttribute, string src)
        {
            return IFrameStart(id, name, classAttribute, src);
        }
        public static HtmlString IFrameStart(
            string id, string name, string classAttribute, string src)
        {
            var tagBuilder = new TagBuilder("iframe");
            tagBuilder.MergeAttribute("id", id);
            tagBuilder.MergeAttribute("name", name);
            if (!string.IsNullOrEmpty(classAttribute))
            {
                tagBuilder.MergeAttribute("class", classAttribute);
            }
            if (!string.IsNullOrEmpty(src))
            {
                tagBuilder.MergeAttribute("src", src);
            }
            tagBuilder.TagRenderMode = TagRenderMode.StartTag;
            return (WriteTagBuilderHtml(tagBuilder));
        }
        public static HtmlString IFrameEnd(this IHtmlHelper helper)
        {
            return IFrameEnd();
        }
        public static HtmlString IFrameEnd()
        {
            var tagBuilder = new TagBuilder("iframe");
            tagBuilder.TagRenderMode = TagRenderMode.EndTag;
            return (WriteTagBuilderHtml(tagBuilder));
        }
        public static HtmlString Option(this IHtmlHelper helper, string text,
            string value, bool isSelected)
        {
            return Option(text, value, isSelected);
        }
        
        public static HtmlString Option(string text,
            string value, bool isSelected)
        {
            var tagBuilder = new TagBuilder("option");
            tagBuilder.MergeAttribute("value", value);
            if (isSelected)
            {
                tagBuilder.MergeAttribute("selected", "selected");
            }
            tagBuilder.InnerHtml.AppendHtml(text);
            tagBuilder.TagRenderMode = TagRenderMode.Normal;
            return (WriteTagBuilderHtml(tagBuilder));
        }
        
        public static HtmlString StrongStart(this IHtmlHelper helper)
        {
            return StrongStart();
        }
        public static HtmlString StrongStart()
        {
            return new HtmlString("<strong>");
        }
        public static HtmlString StrongEnd(this IHtmlHelper helper)
        {
            return StrongEnd();
        }
        public static HtmlString StrongEnd()
        {
            return new HtmlString("</strong>");
        }
        public static HtmlString H3(this IHtmlHelper helper, string text, string classAtt)
        {
            return H3(text, classAtt);
        }
        public static HtmlString H3(string text, string classAtt)
        {
            var tagBuilder = new TagBuilder("h3");
            if (classAtt != string.Empty)
            {
                tagBuilder.MergeAttribute("class", classAtt);
            }
            tagBuilder.InnerHtml.AppendHtml(text);
            tagBuilder.TagRenderMode = TagRenderMode.Normal;
            return (WriteTagBuilderHtml(tagBuilder));
        }
        public static HtmlString H4Start(this IHtmlHelper helper)
        {
            return H4Start();
        }
        public static HtmlString H4Start()
        {
            return new HtmlString("<h4>");
        }
        public static HtmlString H4(this IHtmlHelper helper, string text, string classAtt)
        {
            return H4(text, classAtt);
        }
        public static HtmlString H4(string text, string classAtt)
        {
            var tagBuilder = new TagBuilder("h4");
            if (classAtt != string.Empty)
            {
                tagBuilder.MergeAttribute("class", classAtt);
            }
            tagBuilder.InnerHtml.AppendHtml(text);
            tagBuilder.TagRenderMode = TagRenderMode.Normal;
            return (WriteTagBuilderHtml(tagBuilder));
        }
        public static HtmlString H4End(this IHtmlHelper helper)
        {
            return H4End();
        }
        public static HtmlString H4End()
        {
            return new HtmlString("</h4>");
        }
        public static HtmlString Meta(this IHtmlHelper helper, string name,
            string content, string charset)
        {
            return Meta(name, content, charset);
        }
        public static HtmlString Meta(string name, 
            string content, string charset)
        {
            var tagBuilder = new TagBuilder("meta");
            if (name != string.Empty)
            {
                tagBuilder.MergeAttribute("name", name);
            }
            if (content != string.Empty)
            {
                tagBuilder.MergeAttribute("content", content);
            }
            if (charset != string.Empty)
            {
                tagBuilder.MergeAttribute("charset", charset);
            }
            tagBuilder.TagRenderMode = TagRenderMode.SelfClosing;
            return (WriteTagBuilderHtml(tagBuilder));
        }
        public static HtmlString LinkHead(this IHtmlHelper helper, string rel,
            string href)
        {
            return LinkHead(rel, href);
        }
        public static HtmlString LinkHead(string rel,
            string href)
        {
            var tagBuilder = new TagBuilder("link");
            if (rel != string.Empty)
            {
                tagBuilder.MergeAttribute("rel", rel);
            }
            if (href != string.Empty)
            {
                tagBuilder.MergeAttribute("href", href);
            }
            tagBuilder.TagRenderMode = TagRenderMode.SelfClosing;
            return (WriteTagBuilderHtml(tagBuilder));
        }
        public static HtmlString ScriptsHead(this IHtmlHelper helper, string src,
            string type, string href)
        {
            return ScriptsHead(src);
        }
        public static HtmlString ScriptsHead(string src)
        {
            var tagBuilder = new TagBuilder("script");
            if (src != string.Empty)
            {
                tagBuilder.MergeAttribute("src", src);
            }
            tagBuilder.TagRenderMode = TagRenderMode.Normal;
            return (WriteTagBuilderHtml(tagBuilder));
        }
        public static HtmlString GetMetaDescription(this IHtmlHelper helper, 
            string desc)
        {
            //seo
            return GetMetaDescription(desc);
        }
        public static HtmlString GetMetaDescription(string desc)
        {
            //seo
            string sMeta = string.Concat("<meta name=",
                DevTreks.Data.RuleHelpers.GeneralRules.DOUBLEQUOTE,
                "description",
                DevTreks.Data.RuleHelpers.GeneralRules.DOUBLEQUOTE,
                " content=",
                DevTreks.Data.RuleHelpers.GeneralRules.DOUBLEQUOTE,
                desc,
                DevTreks.Data.RuleHelpers.GeneralRules.DOUBLEQUOTE,
                " />");
            return new HtmlString(sMeta);
        }
        public static HtmlString DivStart(this IHtmlHelper helper,
            string id, string classAtt)
        {
            return DivStart(id, classAtt);
        }
        public static HtmlString DivStart(string id, string classAtt)
        {
            var tagBuilder = new TagBuilder("div");
            if (id != string.Empty)
            {
                tagBuilder.MergeAttribute("id", id);
            }
            if (classAtt != string.Empty)
            {
                tagBuilder.MergeAttribute("class", classAtt);
            }
            tagBuilder.TagRenderMode = TagRenderMode.StartTag;
            return (WriteTagBuilderHtml(tagBuilder));
        }
        public static HtmlString DivStart(this IHtmlHelper helper,
            string id, string classAtt, string dataRole, string dataType)
        {
            return DivStart(id, classAtt, dataRole, dataType);
        }
        public static HtmlString DivStart(string id, string classAtt, 
            string dataRole, string dataType)
        {
            var tagBuilder = new TagBuilder("div");
            if (id != string.Empty)
            {
                tagBuilder.MergeAttribute("id", id);
            }
            if (classAtt != string.Empty)
            {
                tagBuilder.MergeAttribute("class", classAtt);
            }
            if (dataRole != string.Empty)
            {
                tagBuilder.MergeAttribute("data-role", dataRole);
            }
            if (dataType != string.Empty)
            {
                tagBuilder.MergeAttribute("data-type", dataType);
            }
            tagBuilder.TagRenderMode = TagRenderMode.StartTag;
            return (WriteTagBuilderHtml(tagBuilder));
        }
        public static HtmlString DivCollapseStart(this IHtmlHelper helper,
           DevTreks.Data.ContentURI model, int recordToDisplayId)
        {
            return DivCollapseStart(model, recordToDisplayId);
        }
        public static HtmlString DivCollapseStart(
           ContentURI model, int recordToDisplayId)
        {
            bool bIsCollapsed = true;
            if (model.URIDataManager.ServerSubActionType 
                == GeneralHelpers.SERVER_SUBACTION_TYPES.submitedits)
            {
                bIsCollapsed = StylesheetHelper.IsCollapsed(model, recordToDisplayId);
            }
            if (bIsCollapsed)
            {
                return DivStart(string.Concat(GeneralHelpers.CLP, recordToDisplayId.ToString()),
                    string.Empty, "collapsible", "b", "d", string.Empty, "true");
            }
            else
            {
                return DivStart(string.Concat(GeneralHelpers.CLP, recordToDisplayId.ToString()),
                    string.Empty, "collapsible", "b", "d", "false", "true");
            }
        }
        public static HtmlString DivStart(this IHtmlHelper helper,
            string id, string classAtt, string dataRole, string dataTheme,
            string dataContentTheme, string dataCollapsed, string dataMini)
        {
            return (DivStart(id, classAtt, dataRole, dataTheme,
                dataContentTheme, dataCollapsed, dataMini));
        }
        public static HtmlString DivStart(
            string id, string classAtt, string dataRole, string dataTheme, 
            string dataContentTheme, string dataCollapsed, string dataMini)
        { 
            var tagBuilder = new TagBuilder("div");
            if (id != string.Empty)
            {
                tagBuilder.MergeAttribute("id", id);
            }
            if (dataRole != string.Empty)
            {
                tagBuilder.MergeAttribute("data-role", dataRole);
            }
            if (dataTheme != string.Empty)
            {
                tagBuilder.MergeAttribute("data-theme", dataTheme);
            }
            if (dataContentTheme != string.Empty)
            {
                tagBuilder.MergeAttribute("data-content-theme", dataContentTheme);
            }
            if (dataCollapsed != string.Empty)
            {
                tagBuilder.MergeAttribute("data-collapsed", dataCollapsed);
            }
            if (dataMini != string.Empty)
            {
                tagBuilder.MergeAttribute("data-mini", dataMini);
            }
            tagBuilder.TagRenderMode = TagRenderMode.StartTag;
            return (WriteTagBuilderHtml(tagBuilder));
        }
        public static HtmlString DivItemStart(this IHtmlHelper helper,
            string id, string itemprop, string itemscope, string itemtype)
        {
            return DivItemStart(id, itemprop, itemscope, itemtype);
        }
        public static HtmlString DivItemStart(string id, 
            string itemprop, string itemscope, string itemtype)
        {
            var tagBuilder = new TagBuilder("div");
            if (id != string.Empty)
            {
                tagBuilder.MergeAttribute("id", id);
            }
            if (itemprop != string.Empty)
            {
                tagBuilder.MergeAttribute("itemprop", itemprop);
            }
            if (itemscope != string.Empty)
            {
                tagBuilder.MergeAttribute("itemscope", string.Empty);
            }
            if (itemtype != string.Empty)
            {
                tagBuilder.MergeAttribute("itemtype", itemtype);
            }
            tagBuilder.TagRenderMode = TagRenderMode.StartTag;
            return (WriteTagBuilderHtml(tagBuilder));
        }
        public static HtmlString SpanItemStart(this IHtmlHelper helper,
            string itemprop)
        {
            return SpanItemStart(itemprop);
        }
        public static HtmlString SpanItemStart(string itemprop)
        {
            var tagBuilder = new TagBuilder("span");
            if (itemprop != string.Empty)
            {
                tagBuilder.MergeAttribute("itemprop", itemprop);
            }
            tagBuilder.TagRenderMode = TagRenderMode.StartTag;
            return (WriteTagBuilderHtml(tagBuilder));
        }
        public static HtmlString MetaItem(this IHtmlHelper helper,
            string id, string itemprop, string content)
        {
            return MetaItem(id, itemprop, content);
        }
        public static HtmlString MetaItem(
            string id, string itemprop, string content)
        {
            var tagBuilder = new TagBuilder("meta");
            if (id != string.Empty)
            {
                tagBuilder.MergeAttribute("id", id);
            }
            if (itemprop != string.Empty)
            {
                tagBuilder.MergeAttribute("itemprop", itemprop);
            }
            if (content != string.Empty)
            {
                tagBuilder.MergeAttribute("content", content);
            }
            tagBuilder.TagRenderMode = TagRenderMode.SelfClosing;
            return (WriteTagBuilderHtml(tagBuilder));
        }
        public static HtmlString VideoItemStart(this IHtmlHelper helper,
            string controls, string poster, string width, string height,
            string preload)
        {
            return VideoItemStart(controls,
                poster, width, height, preload);
        }
        public static HtmlString VideoItemStart(string controls, 
            string poster, string width, string height, 
            string preload)
        {
            var tagBuilder = new TagBuilder("video");
            if (controls != string.Empty)
            {
                tagBuilder.MergeAttribute("controls", string.Empty);
            }
            if (poster != string.Empty)
            {
                tagBuilder.MergeAttribute("poster", poster);
            }
            if (width != string.Empty)
            {
                tagBuilder.MergeAttribute("width", width);
            }
            if (height != string.Empty)
            {
                tagBuilder.MergeAttribute("height", height);
            }
            if (preload != string.Empty)
            {
                tagBuilder.MergeAttribute("preload", preload);
            }
            tagBuilder.TagRenderMode = TagRenderMode.StartTag;
            return (WriteTagBuilderHtml(tagBuilder));
        }
        public static HtmlString VideoEnd(this IHtmlHelper helper)
        {
            return VideoEnd();
        }
        public static HtmlString VideoEnd()
        {
            return new HtmlString("</video>");
        }
        public static HtmlString SourceItem(this IHtmlHelper helper,
            string src, string type)
        {
            return SourceItem(src, type);
        }
        public static HtmlString SourceItem(string src, string type)
        {
            var tagBuilder = new TagBuilder("source");
            if (src != string.Empty)
            {
                tagBuilder.MergeAttribute("src", src);
            }
            if (type != string.Empty)
            {
                tagBuilder.MergeAttribute("type", type);
            }
            tagBuilder.TagRenderMode = TagRenderMode.SelfClosing;
            return (WriteTagBuilderHtml(tagBuilder));
        }
        public static HtmlString LinkItemStart(this IHtmlHelper helper,
            string id, string itemprop, string href)
        {
            return LinkItemStart(id, itemprop, href);
        }
        public static HtmlString LinkItemStart(string id, string itemprop, string href)
        {
            var tagBuilder = new TagBuilder("a");
            if (id != string.Empty)
            {
                tagBuilder.MergeAttribute("id", id);
            }
            if (itemprop != string.Empty)
            {
                tagBuilder.MergeAttribute("itemprop", itemprop);
            }
            if (href != string.Empty)
            {
                tagBuilder.MergeAttribute("href", href);
            }
            tagBuilder.TagRenderMode = TagRenderMode.StartTag;
            return (WriteTagBuilderHtml(tagBuilder));
        }
        public static HtmlString LinkEnd(this IHtmlHelper helper)
        {
            return LinkEnd();
        }
        public static HtmlString LinkEnd()
        {
            return new HtmlString("</a>");
        }
        public static HtmlString DivEnd(this IHtmlHelper helper)
        {
            return DivEnd();
        }
        public static HtmlString DivEnd()
        {
            return new HtmlString("</div>");
        }
        public static HtmlString FieldsetStart(this IHtmlHelper helper,
            string id, string classAtt)
        {
            return FieldsetStart(id, classAtt);
        }
        public static HtmlString FieldsetStart(string id, string classAtt)
        {
            var tagBuilder = new TagBuilder("fieldset");
            if (id != string.Empty)
            {
                tagBuilder.MergeAttribute("id", id);
            }
            if (classAtt != string.Empty)
            {
                tagBuilder.MergeAttribute("class", classAtt);
            }
            tagBuilder.TagRenderMode = TagRenderMode.StartTag;
            return (WriteTagBuilderHtml(tagBuilder));
        }
        public static HtmlString FieldsetStart(this IHtmlHelper helper,
            string id, string classAtt, string dataRole, string dataType,
            string dataMini)
        {
            return FieldsetStart(id, classAtt, dataRole, dataType,
            dataMini);
        }
        public static HtmlString FieldsetStart(
            string id, string classAtt, string dataRole, string dataType, 
            string dataMini)
        {
            var tagBuilder = new TagBuilder("fieldset");
            if (id != string.Empty)
            {
                tagBuilder.MergeAttribute("id", id);
            }
            if (classAtt != string.Empty)
            {
                tagBuilder.MergeAttribute("class", classAtt);
            }
            if (dataRole != string.Empty)
            {
                tagBuilder.MergeAttribute("data-role", dataRole);
            }
            if (dataType != string.Empty)
            {
                tagBuilder.MergeAttribute("data-type", dataType);
            }
            if (dataMini != string.Empty)
            {
                tagBuilder.MergeAttribute("data-mini", dataMini);
            }
            tagBuilder.TagRenderMode = TagRenderMode.StartTag;
            return (WriteTagBuilderHtml(tagBuilder));
        }
        public static HtmlString FieldsetEnd(this IHtmlHelper helper)
        {
            return FieldsetEnd();
        }
        public static HtmlString FieldsetEnd()
        {
            return new HtmlString("</fieldset>");
        }
        public static HtmlString Legend(this IHtmlHelper helper,
            string text)
        {
            return Legend(text);
        }
        public static HtmlString Legend(string text)
        {
            var tagBuilder = new TagBuilder("legend");
            tagBuilder.TagRenderMode = TagRenderMode.StartTag;
            tagBuilder.InnerHtml.Append(text);
            tagBuilder.TagRenderMode = TagRenderMode.Normal;
            return (WriteTagBuilderHtml(tagBuilder));
        }
        public static HtmlString Span(this IHtmlHelper helper,
            string id, string classAtt, string text)
        {
            return Span(id, classAtt, text);
        }
        public static HtmlString Span(string id, string classAtt, string text)
        {
            var tagBuilder = new TagBuilder("span");
            if (id != string.Empty)
            {
                tagBuilder.MergeAttribute("id", id);
            }
            if (classAtt != string.Empty)
            {
                tagBuilder.MergeAttribute("class", classAtt);
            }
            tagBuilder.InnerHtml.Append(text);
            tagBuilder.TagRenderMode = TagRenderMode.Normal;
            return (WriteTagBuilderHtml(tagBuilder));
        }
        public static HtmlString SpanStart(this IHtmlHelper helper,
            string id, string classAtt)
        {
            return SpanStart(id, classAtt);
        }
        public static HtmlString SpanStart(string id, string classAtt)
        {
            var tagBuilder = new TagBuilder("span");
            if (id != string.Empty)
            {
                tagBuilder.MergeAttribute("id", id);
            }
            if (classAtt != string.Empty)
            {
                tagBuilder.MergeAttribute("class", classAtt);
            }
            tagBuilder.TagRenderMode = TagRenderMode.StartTag;
            return (WriteTagBuilderHtml(tagBuilder));
        }
        public static HtmlString SpanEnd(this IHtmlHelper helper)
        {
            return SpanEnd();
        }
        public static HtmlString SpanEnd()
        {
            return new HtmlString("</span>");
        }
        public static HtmlString SpanStrong(this IHtmlHelper helper,
            string id, string classAtt, string text)
        {
            return SpanStrong(id, classAtt, text);
        }
        public static HtmlString SpanStrong(string id, string classAtt, string text)
        {
            var tagBuilder = new TagBuilder("span");
            if (id != string.Empty)
            {
                tagBuilder.MergeAttribute("id", id);
            }
            if (classAtt != string.Empty)
            {
                tagBuilder.MergeAttribute("class", classAtt);
            }
            tagBuilder.InnerHtml.AppendHtml(string.Concat("<strong>", text, "</strong>"));
            tagBuilder.TagRenderMode = TagRenderMode.Normal;
            return (WriteTagBuilderHtml(tagBuilder));
        }
        public static HtmlString SpanError(this IHtmlHelper helper,
            string errorMessage)
        {
            return SpanError(errorMessage);
        }
        public static HtmlString SpanError(string errorMessage)
        {
            var tagBuilder = new TagBuilder("span");
            tagBuilder.MergeAttribute("id", DevTreks.Exceptions.DevTreksErrors.DISPLAY_ERROR_ID2);
            tagBuilder.InnerHtml.AppendHtml(string.Concat("<strong>", errorMessage, "</strong>"));
            tagBuilder.TagRenderMode = TagRenderMode.Normal;
            return (WriteTagBuilderHtml(tagBuilder));
        }
        public static HtmlString PError(this IHtmlHelper helper,
            string errorMessage)
        {
            return PError(errorMessage);
        }
        public static HtmlString PError(string errorMessage)
        {
            var tagBuilder = new TagBuilder("p");
            tagBuilder.MergeAttribute("id", DevTreks.Exceptions.DevTreksErrors.DISPLAY_ERROR_ID2);
            tagBuilder.InnerHtml.AppendHtml(string.Concat("<strong>", errorMessage, "</strong>"));
            tagBuilder.TagRenderMode = TagRenderMode.Normal;
            return (WriteTagBuilderHtml(tagBuilder));
        }
      
        public static HtmlString FormStart(this IHtmlHelper helper,
           string id, string action, string method, string encType)
        {
            return FormStart(id, action, method, encType);
        }
        public static HtmlString FormStart(string id, 
            string action, string method, string encType)
        {
            var tagBuilder = new TagBuilder("form");
            tagBuilder.MergeAttribute("id", id);
            if (method != string.Empty)
            {
                tagBuilder.MergeAttribute("method", method);
            }
            tagBuilder.MergeAttribute("action", action);
            if (encType != string.Empty)
            {
                tagBuilder.MergeAttribute("encType", encType);
            }
            tagBuilder.TagRenderMode = TagRenderMode.StartTag;
            return (WriteTagBuilderHtml(tagBuilder));
        }
        public static HtmlString FormStart(this IHtmlHelper helper,
           string id, string action)
        {
            return FormStart(id, action);
        }
        public static HtmlString FormStart(string id, string action)
        {
            var tagBuilder = new TagBuilder("form");
            tagBuilder.MergeAttribute("id", id);
            tagBuilder.MergeAttribute("action", action);
            tagBuilder.TagRenderMode = TagRenderMode.StartTag;
            return (WriteTagBuilderHtml(tagBuilder));
        }
        public static HtmlString FormEnd(this IHtmlHelper helper)
        {
            return FormEnd();
        }
        public static HtmlString FormEnd()
        {
            return new HtmlString("</form>");
        }
        public static HtmlString LabelHidden(this IHtmlHelper helper, string forName, string htmlvalue,
            string className)
        {
            return LabelHidden(forName, htmlvalue, className);
        }
        public static HtmlString LabelHidden(string forName, string htmlvalue,
            string className)
        {
            //label won't be shown but is still accessible to machines
            var tagBuilder = new TagBuilder("label");
            tagBuilder.MergeAttribute("for", forName);
            //jquery mobile classname is ui-hidden-accessible
            tagBuilder.MergeAttribute("class", className);
            tagBuilder.InnerHtml.AppendHtml(htmlvalue);
            tagBuilder.TagRenderMode = TagRenderMode.Normal;
            return (WriteTagBuilderHtml(tagBuilder));
        }
        public static HtmlString LabelRegular(this IHtmlHelper helper,
            string forName, string htmlvalue)
        {
            return LabelRegular(forName, htmlvalue);
        }
        public static HtmlString LabelRegular(string forName, string htmlvalue)
        {
            var tagBuilder = new TagBuilder("label");
            tagBuilder.MergeAttribute("for", forName);
            tagBuilder.InnerHtml.AppendHtml(htmlvalue);
            tagBuilder.TagRenderMode = TagRenderMode.Normal;
            return (WriteTagBuilderHtml(tagBuilder));
        }
        public static HtmlString LabelStrong(this IHtmlHelper helper, string forName, string value)
        {
            return LabelStrong(forName, value);
        }
        public static HtmlString LabelStrong(string forName, string value)
        {
            var tagBuilder = new TagBuilder("label");
            tagBuilder.MergeAttribute("for", forName);
            tagBuilder.InnerHtml.AppendHtml(string.Concat("<strong>", value, "</strong>"));
            tagBuilder.TagRenderMode = TagRenderMode.Normal;
            return (WriteTagBuilderHtml(tagBuilder));
        }
        public static HtmlString LabelItalic(this IHtmlHelper helper, string forName, string value)
        {
            return LabelItalic(forName, value);
        }
        public static HtmlString LabelItalic(string forName, string value)
        {
            var tagBuilder = new TagBuilder("label");
            tagBuilder.MergeAttribute("for", forName);
            tagBuilder.InnerHtml.AppendHtml(string.Concat("<em>", value, "</em>"));
            tagBuilder.TagRenderMode = TagRenderMode.Normal;
            return (WriteTagBuilderHtml(tagBuilder));
        }
        public static HtmlString TextArea(this IHtmlHelper helper, string id, string name,
            string className, string value, bool disabled)
        {
            return TextArea(id, name, className, value, disabled);
        }
        public static HtmlString TextArea(string id, string name, 
            string className, string value, bool disabled)
        {
            var tagBuilder = new TagBuilder("textarea");
            tagBuilder.MergeAttribute("id", id);
            tagBuilder.MergeAttribute("name", name);
            if (className != string.Empty)
            {
                tagBuilder.MergeAttribute("class", className);
            }
            if (disabled)
            {
                tagBuilder.MergeAttribute("disabled", string.Empty);
            }
            tagBuilder.InnerHtml.AppendHtml(value);
            tagBuilder.TagRenderMode = TagRenderMode.Normal;
            return (WriteTagBuilderHtml(tagBuilder));
        }
        public static HtmlString HtmlStart(this IHtmlHelper helper)
        {
            return HtmlStart();
        }
        public static HtmlString HtmlStart()
        {
            var tagBuilder = new TagBuilder("html");
            tagBuilder.TagRenderMode = TagRenderMode.StartTag;
            return (WriteTagBuilderHtml(tagBuilder));
        }
        
        public static HtmlString HtmlEnd(this IHtmlHelper helper)
        {
            return HtmlEnd();
        }
        public static HtmlString HtmlEnd()
        {
            return new HtmlString("</html>");
        }
        
        public static HtmlString Select3(this IHtmlHelper helper)
        {
            return Select3();
        }
        public static HtmlString Select3()
        {
            return new HtmlString("</html>");
        }
    }
}