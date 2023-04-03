using System.Web.Mvc;

namespace UniversalWebApi.Controllers
{
    public class CORSDomainAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Origin", "*");
            filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Headers", "*");
            //filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Credentials", "true");
            filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Max-Age", "1000");
            filterContext.RequestContext.HttpContext.Response.AddHeader("Vary", "Origin");
            base.OnActionExecuting(filterContext);
        }
    }
}