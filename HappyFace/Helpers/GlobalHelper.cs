using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace PodBeeMedia.Helpers
{
    public static class GlobalHelper
    {
        public static IEnumerable<string> ReadFile(string path)
        {
            return File.ReadAllLines(HttpContext.Current.Server.MapPath(path));
        }
        public static String ReadCss(string path)
        {
            string css = string.Empty;
            foreach (var line in ReadFile(path))
            {
                css += line;
            }
            return css;
        }
        public static MvcHtmlString SvgIcon(this HtmlHelper helper, string iconName, object htmlAttributes, string className = "")
        {
            className = !String.IsNullOrEmpty(className.Trim()) ? " " + className : className;
            TagBuilder spanTag = new TagBuilder("span");
            spanTag.MergeAttribute("class", "svg-icon" + className);
            TagBuilder svgTag = new TagBuilder("svg");
            RouteValueDictionary htmlAttr = new RouteValueDictionary(htmlAttributes);
            svgTag.MergeAttributes(htmlAttr);
            svgTag.MergeAttribute("viewBox", "0 0 " + htmlAttr["width"].ToString() + " " + htmlAttr["height"].ToString());
            TagBuilder useTag = new TagBuilder("use");
            var xLinkBuild = "/Public/svg/icons.svg#" + iconName;
            useTag.MergeAttribute("xlink:href", xLinkBuild);
            svgTag.InnerHtml = useTag.ToString(TagRenderMode.SelfClosing);
            spanTag.InnerHtml = svgTag.ToString();
            return MvcHtmlString.Create(spanTag.ToString());
        }
        public static string GetFilePath(string directory, string name, HttpPostedFileBase file)
        {
            return Path.Combine(HttpContext.Current.Server.MapPath(directory), CreateUrl(name) + Path.GetExtension(file.FileName));
        }
        public static string CreateUrl(string text)
        {
            char[] oldChar = new char[] { '!', '\'', '^', '+', '%', '&', '/', '(', ')', '=', '?', ';', '>', '£', '#', '$', '½', '{', '[', ']', '}', '*', '|', '`', '.', '¨', '~', '´', ':', '/', ' ', '&', 'ü', 'Ü', 'Ş', 'ş', 'Ö', 'ö', 'Ç', 'ç', ',', 'İ', 'ı' };
            char[] newChar = new char[] { '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', 'u', 'U', 'S', 's', 'O', 'o', 'C', 'c', '-', 'I', 'i' };
            for (int i = 0; i < oldChar.Length; i++)
            {
                text = text.Replace(oldChar[i], newChar[i]);
            }
            return text.ToLower(new CultureInfo("en-US", false)).Replace("---", "-").Replace("--", "-");
        }
        public static string Encrypt(string text)
        {
            MD5 encrypt = new MD5CryptoServiceProvider();
            encrypt.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));
            byte[] result = encrypt.Hash;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                sb.Append(result[i].ToString("x2"));
            }
            return sb.ToString();
        }
        public static string ClearStyle(string text)
        {
            RegexOptions options = RegexOptions.Multiline;
            return Regex.Replace(text, "style=[\"'](.*)[\"']", "", options);
        }
    }
}