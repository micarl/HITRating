using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HitRating.HtmlHelpers
{
    public static class UrlHelpers
    {
        public static string ParseUrl(this HtmlHelper helper, string relativeUrl)
        {
            return Models.ApplicationConfig.Domain + relativeUrl;
        }

        public static string ParseImageUrl(this HtmlHelper helper, string relativeImageUrl)
        {
            return ParseUrl(helper, relativeImageUrl);
        }

        public static string ParseCssUrl(this HtmlHelper helper, string relativeCssUrl)
        {
            return ParseUrl(helper, relativeCssUrl);
        }

        public static string ParseJsUrl(this HtmlHelper helper, string relativeJsUrl)
        {
            return ParseUrl(helper, relativeJsUrl);
        }

        public static MvcHtmlString ActionLinkWithImage(this HtmlHelper html, string imgSrc, string actionName, string controllerName, object routeValue = null, string alt = null, string target = null)
        {
            var urlHelper = new UrlHelper(html.ViewContext.RequestContext);

            string imgUrl = urlHelper.Content(imgSrc);
            TagBuilder imgTagBuilder = new TagBuilder("img");
            imgTagBuilder.MergeAttribute("src", imgUrl);
            imgTagBuilder.MergeAttribute("alt", alt);
            imgTagBuilder.MergeAttribute("title", alt);
            string img = imgTagBuilder.ToString(TagRenderMode.SelfClosing);

            string url = urlHelper.Action(actionName, controllerName, routeValue);

            TagBuilder tagBuilder = new TagBuilder("a")
            {
                InnerHtml = img
            };
            tagBuilder.MergeAttribute("href", url);
            tagBuilder.MergeAttribute("target", target); 

            return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.Normal));
        }
    }
}
