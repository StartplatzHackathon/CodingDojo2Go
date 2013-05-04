using System.Web;
using System.Web.Http.Filters;
using Elmah;

namespace CodingKata2Go.WebServices.Filter
{
    public class UnhandledExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            ErrorLog.GetDefault(HttpContext.Current).Log(new Error(context.Exception));
        }
    }
}