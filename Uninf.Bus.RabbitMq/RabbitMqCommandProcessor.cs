namespace Uninf.Bus.RabbitMQ
{
    using System.Transactions;

    using global::RabbitMQ.Client;

    public class RabbitMqCommandProcessor<TCommand> : ICommandProcessor<TCommand> where TCommand : ICommand
    {
        private IRabbitMqConfig server;

        private IRabbitMqCommandSendConfigGetter configGetter;

        public RabbitMqCommandProcessor(IRabbitMqConfig server, IRabbitMqCommandSendConfigGetter configGetter)
        {
            this.server = server;
            this.configGetter = configGetter;
        }

        public void Process(TCommand command)
        {
            var factory = new ConnectionFactory() { Uri = server.ConnectionString };
            var sendConfig = configGetter.GetEventSendConfig<TCommand>();
            var connection = factory.CreateConnection();

            var channel = connection.CreateModel();
            if (Transaction.Current != null)
            {
                new RabbitMqResourceManager(connection,channel, Transaction.Current);
            }
            var body = sendConfig.Serializ(command);
            channel.BasicPublish(sendConfig.Exchange(command), sendConfig.RoutetingKey(command), null, body);

            if (Transaction.Current == null)
            {
                channel.Dispose();
                connection.Dispose();
            }
        }
    }

    public interface IRabbitMqCommandSendConfigGetter
    {
        IRabbitMqCommandSendConfig<T> GetEventSendConfig<T>() where T : ICommand;
    }

    public interface IRabbitMqCommandSendConfig<T> where T : ICommand
    {
        string Exchange(T evt);

        string RoutetingKey(T evt);

        byte[] Serializ(T evt);

        T Deserializ(string str);
    }
}