using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Html;
using System;
using System.Linq;

namespace DocSpider.Helpers
{
    public static class HtmlExtensions
    {
        public static string IsActive(this IHtmlHelper html, string controller, params string[] actions)
        {
            var routeData = html.ViewContext.RouteData;
            var routeAction = routeData.Values["action"]?.ToString();
            var routeController = routeData.Values["controller"]?.ToString();

            bool isActive = routeController == controller && actions.Contains(routeAction);

            return isActive ? "active" : "";
        }
    }
}