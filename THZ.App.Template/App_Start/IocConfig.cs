namespace THZ.App.Template
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Reflection;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.WebHost;
    using System.Web.Mvc;

    using Autofac;
    using Autofac.Extras.CommonServiceLocator;
    using Autofac.Integration.Mvc;
    using Autofac.Integration.WebApi;

    using Microsoft.Practices.ServiceLocation;

    using THZ.App.Template.Auth;
    using THZ.App.Template.Bus;
    using THZ.App.Template.Config;
    using THZ.App.Template.Database;
    using THZ.App.Template.Helpers;

    using Uninf.Auth;
    using Uninf.Bus;
    using Uninf.Bus.RabbitMq;
    using Uninf.Bus.THZ;
    using Uninf.Cache.Redis;
    using Uninf.CacheData;
    using Uninf.Config;
    using Uninf.Model;
    using Uninf.Model.Automapper;

    public class IocConfig
    {
        public static void Config(AppConfig config)
        {
            var builder = new ContainerBuilder();

            RegisterAuth(builder);

            RegisterCache(builder, config);

            RegisterBus(builder, config);

            RegisterSql(builder);

            RegisterHandlers(builder);

            RegisterGetterSetter(builder);

            RegisterMapper(builder);

            RegisterWeb(builder);

            builder.RegisterType<THZHelper>().AsImplementedInterfaces().InstancePerRequest();
            builder.RegisterType<AppHelper>().AsImplementedInterfaces().InstancePerRequest();

            var container = builder.Build();
            //mvc
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            //web api
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            GlobalConfiguration.Configuration.MessageHandlers.Add(new CommonServiceLocatorApiHandler());
            //common service locator
            var csl = new AutofacServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() =>
                {
                    var httpContext = HttpContext.Current;
                    if (httpContext.CurrentHandler is MvcHandler)
                    {
                        return new AutofacServiceLocator(AutofacDependencyResolver.Current.RequestLifetimeScope);
                    }
                    else if (httpContext.CurrentHandler is HttpControllerHandler)
                    {
                        var handler =
                            GlobalConfiguration.Configuration.MessageHandlers.FirstOrDefault(
                                x => x is CommonServiceLocatorApiHandler) as CommonServiceLocatorApiHandler;
                        if (handler != null)
                        {
                            return new AutofacServiceLocator(handler.Request.GetDependencyScope().GetRequestLifetimeScope());
                        }
                    }
                    return csl;
                });
        }

        private static void RegisterAuth(ContainerBuilder builder)
        {
            builder.RegisterType<THZCookieStorage>().AsImplementedInterfaces();
            builder.RegisterType<THZDesEncryptor>().AsImplementedInterfaces();
            builder.RegisterType<THZUserLoginSerializer>().AsImplementedInterfaces();
            builder.RegisterType<THZAuthOperator>().AsImplementedInterfaces();
        }

        private static void RegisterCache(ContainerBuilder builder, AppConfig config)
        {
            builder.RegisterInstance(config.RedisConfig).As<IRedisConfig>().ExternallyOwned();
            builder.RegisterType<RedisCache>().AsImplementedInterfaces().InstancePerRequest();
        }

        private static void RegisterWeb(ContainerBuilder builder)
        {
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterFilterProvider();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            var config = GlobalConfiguration.Configuration;
            builder.RegisterWebApiFilterProvider(config);
            builder.RegisterHttpRequestMessage(config);
        }

        private static void RegisterSql(ContainerBuilder builder)
        {
            builder.RegisterType<DatabaseContext>().AsImplementedInterfaces().InstancePerRequest();
        }

        private static void RegisterBus(ContainerBuilder builder, AppConfig config)
        {
            builder.RegisterInstance(config.QueueConfig).As<IRabbitServerConfig>().ExternallyOwned();
            builder.RegisterType<THZBus>().AsImplementedInterfaces().InstancePerRequest();
            builder.RegisterType<JsonMessageSerializer>().AsImplementedInterfaces();
            builder.RegisterType<HandlerThreadStore>().AsImplementedInterfaces();

            builder.RegisterType<THZListener>().AsImplementedInterfaces();
        }

        private static void RegisterHandlers(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(x => x.IsAssignableTo<IHandler>())
                .AsImplementedInterfaces().AsSelf();
        }

        private static void RegisterGetterSetter(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(x => x.IsAssignableTo<IGetter>() || x.IsAssignableTo<ISetter>())
                .AsImplementedInterfaces().AsSelf();
        }

        private static void RegisterMapper(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(x => x.IsAssignableTo<IModelConverterRegister>())
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(x => x.IsAssignableTo<IModelConverter>())
                .AsImplementedInterfaces();
        }
    }

    public sealed class CommonServiceLocatorApiHandler : DelegatingHandler
    {
        public HttpRequestMessage Request { get; private set; }

        protected override System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            Request = request;
            return base.SendAsync(request, cancellationToken);
        }
    }
}