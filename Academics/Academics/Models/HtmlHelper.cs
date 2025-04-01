using Microsoft.AspNetCore.Mvc.Rendering;

namespace Academics.Models
{
    public static class HtmlHelper
    {
        public static string IsActive(this IHtmlHelper htmlHelper, string controller)
        {
            var routeData = htmlHelper.ViewContext.RouteData;
            var currentController = routeData.Values["controller"]?.ToString();
            return currentController == controller ? "active" : "";
        }
    }
}
