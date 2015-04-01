namespace Uninf.Bus.RabbitMQ.Ioc
{
    using Microsoft.Practices.ServiceLocation;

    public class IocRabbitMqCommandSendConfigGetter : IRabbitMqCommandSendConfigGetter
    {
        public IRabbitMqCommandSendConfig<T> GetEventSendConfig<T>() where T : ICommand
        {
            return ServiceLocator.Current.GetInstance<IRabbitMqCommandSendConfig<T>>();
        }
    }
}