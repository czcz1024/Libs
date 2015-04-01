namespace THZ.App.Template.Utility.IocGetter
{
    using Autofac;
    using Autofac.Integration.Mvc;

    using Microsoft.Practices.ServiceLocation;

    public static class CommonServiceLocatorExtensions
    {
        public static T GetInstanceByName<T>(this IServiceLocator lct,string name)
        {
            return AutofacDependencyResolver.Current.RequestLifetimeScope.ResolveNamed<T>(name);
        }
    }
}