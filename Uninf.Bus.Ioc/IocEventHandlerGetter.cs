namespace Uninf.Bus.Ioc
{
    using System.Collections.Generic;

    using Microsoft.Practices.ServiceLocation;

    public class IocEventHandlerGetter : IEventHandlerGetter
    {
        public IEnumerable<IEventHandler<T>> GetEventHandlers<T>() where T : IEvent
        {
            return ServiceLocator.Current.GetAllInstances<IEventHandler<T>>();
        }
    }
}