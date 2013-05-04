using System.Web;
using System.Web.Mvc;

namespace CodingKata2Go.WebServices
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}