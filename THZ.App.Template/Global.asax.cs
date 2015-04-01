using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace THZ.App.Template
{
    using Microsoft.Practices.ServiceLocation;

    using THZ.App.Template.Bus;
    using THZ.App.Template.Config;

    using Uninf.Config;
    using Uninf.Config.PageTemplate;
    using Uninf.Model.Automapper;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            IocConfig.Config(THZConfigHelper<AppConfig>.Instance);
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            //生成配置view
            var path =Server.MapPath("~/Views/Config/Index.cshtml");
            var postTo = "/Config/Index";
            ConfigCshtmlCreator.CreateCshtml<AppConfig>(path, postTo);

            THZBus.StartAll();

            var rs = ServiceLocator.Current.GetAllInstances<IModelConverterRegister>();
            foreach (var r in rs)
            {
                r.Regist();
            }
        }
    }
}
