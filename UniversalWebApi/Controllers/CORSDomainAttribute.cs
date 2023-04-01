using System.Web.Http.Filters;

namespace UniversalWebApi.Controllers
{
    public class CORSDomainAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Response != null)
            {
                actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
                actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization");
            }

            base.OnActionExecuted(actionExecutedContext);
        }
    }
}