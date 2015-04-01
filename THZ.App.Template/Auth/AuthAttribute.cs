namespace THZ.App.Template.Auth
{
    using System.Web;
    using System.Web.Mvc;

    using Uninf.Auth;

    public class UserAuthAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            //return base.AuthorizeCore(httpContext);
            return AuthManagers<THZUserLogin>.IsLog();
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("http://www.tuohuangzu.com/User/Login?ReturnUrl="+HttpUtility.UrlEncode(filterContext.RequestContext.HttpContext.Request.Url.ToString()));
            //base.HandleUnauthorizedRequest(filterContext);
        }
    }
}