namespace Uninf.Bus.RabbitMQ.Ioc
{
    using Microsoft.Practices.ServiceLocation;

    public class IocRabbitMqEventSendConfigGetter : IRabbitMqEventSendConfigGetter
    {
        public IRabbitMqEventSendConfig<T> GetEventSendConfig<T>() where T : IEvent
        {
            return ServiceLocator.Current.GetInstance<IRabbitMqEventSendConfig<T>>();
        }
    }
}