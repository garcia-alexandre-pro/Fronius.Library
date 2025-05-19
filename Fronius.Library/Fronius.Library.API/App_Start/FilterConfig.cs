using System.Web;
using System.Web.Mvc;

namespace Fronius.Library.API
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());

            // Web API configuration and services
            filters.Add(new RequireHttpsAttribute());
        }
    }
}
