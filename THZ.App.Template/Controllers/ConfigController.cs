namespace THZ.App.Template.Controllers
{
    using System;

    using Microsoft.Practices.ServiceLocation;

    using THZ.App.Template.Config;

    using Uninf.Bus;
    using Uninf.Config;
    using Uninf.Config.Mvc5;

    public class ConfigController:ConfigControllerBase<AppConfig>
    {
        protected override void TellOtherServerReloadConfig()
        {
            //如果多台机器负载，需要实现此方法
        }

        private bool X;
        protected override void BeforChange(System.Web.Mvc.FormCollection form, string section)
        {
            X = THZConfigHelper<AppConfig>.Instance.QueueConfig.PubFriendHandler.Async;
        }

        protected override void SaveSuccess(System.Web.Mvc.FormCollection form, string section)
        {
            if (!X)
            {
                if (THZConfigHelper<AppConfig>.Instance.QueueConfig.PubFriendHandler.Async)
                {
                    var name = THZConfigHelper<AppConfig>.Instance.QueueConfig.PubFriendHandler.Name;
                    var t = Type.GetType(name);
                    ServiceLocator.Current.GetInstance<IListener>()
                        .Start(ServiceLocator.Current.GetInstance(t) as IHandler);
                }
            }
        }
    }
}