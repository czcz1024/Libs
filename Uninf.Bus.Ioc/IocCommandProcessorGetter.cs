namespace Uninf.Bus.Ioc
{
    using Microsoft.Practices.ServiceLocation;

    public class IocCommandProcessorGetter : ICommandProcessorGetter
    {
        public ICommandProcessor<T> GetCommandProcessor<T>() where T : ICommand
        {
            return ServiceLocator.Current.GetInstance<ICommandProcessor<T>>();
        }

        public ICommandProcessor<T, TResult> GetCommandProcessor<T, TResult>() where T : ICommand
        {
            return ServiceLocator.Current.GetInstance<ICommandProcessor<T,TResult>>();
        }
    }
}