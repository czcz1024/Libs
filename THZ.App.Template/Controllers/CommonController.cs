namespace THZ.App.Template.Controllers
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web.Mvc;

    using Microsoft.Practices.ServiceLocation;

    using THZ.App.Template.Auth;

    using Uninf.Auth;

    public class CommonController : Controller
    {
        private IAuthOperator<THZUserLogin> auth;

        public CommonController(IAuthOperator<THZUserLogin> auth)
        {
            this.auth = auth;
        }

        public ActionResult Header()
        {
            var cookieValue = this.auth.Storager().Load(this.auth.StorageName());

            var url = "http://www.tuohuangzu.com/baselayout/publicheader";
            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { })
            {
                cookieContainer.Add(new Uri("http://www.tuohuangzu.com"), new Cookie(this.auth.StorageName(), cookieValue));
                var result = client.GetStringAsync(url).Result;
                result = result
                    .Replace("<img src=\"", "<img src=\"http://www.tuohuangzu.com")
                    .Replace("var url = \"", "var url = \"http://www.tuohuangzu.com")
                    .Replace("ajax-url=\"", "ajax-url=\"http://www.tuohuangzu.com")
                    .Replace("href=\"", "href=\"http://www.tuohuangzu.com").Replace("http://www.tuohuangzu.comhttp://", "http://")
                    .Replace("http://www.tuohuangzu.comjavascript:;", "javascript:;")
                    .Replace("http://www.tuohuangzu.com#", "#");
                return this.Content(result);
            }
        }

        public ActionResult Footer()
        {
            var cookieValue = this.auth.Storager().Load(this.auth.StorageName());

            var url = "http://www.tuohuangzu.com/baselayout/publicfooter";
            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { })
            {
                cookieContainer.Add(new Uri("http://www.tuohuangzu.com"), new Cookie(this.auth.StorageName(), cookieValue));
                var result = client.GetStringAsync(url).Result;
                result = result
                    .Replace("<img src=\"", "<img src=\"http://www.tuohuangzu.com")
                    .Replace("var url = \"", "var url = \"http://www.tuohuangzu.com")
                    .Replace("ajax-url=\"", "ajax-url=\"http://www.tuohuangzu.com")
                    .Replace("href=\"", "href=\"http://www.tuohuangzu.com")
                    .Replace("<script src=\"","<script src=\"http://www.tuohuangzu.com")
                    .Replace("<link href=\"","<link href=\"http://www.tuohuangzu.com")
                    .Replace("http://www.tuohuangzu.comhttp://", "http://")
                    .Replace("http://www.tuohuangzu.comjavascript:;", "javascript:;");
                return this.Content(result);
            }
        }
    }
}