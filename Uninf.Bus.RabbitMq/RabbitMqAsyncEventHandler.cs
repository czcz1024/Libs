namespace Uninf.Bus.RabbitMQ
{
    using System.Text;
    using System.Transactions;

    using global::RabbitMQ.Client;

    public class RabbitMqAsyncEventHandler : IAsyncEventHandler
    {
        private IRabbitMqConfig server;

        private IRabbitMqEventSendConfigGetter configGetter;

        public RabbitMqAsyncEventHandler(IRabbitMqConfig server, IRabbitMqEventSendConfigGetter configGetter)
        {
            this.server = server;
            this.configGetter = configGetter;
        }

        public void Handler<T>(T evt) where T : IEvent
        {
            var factory = new ConnectionFactory() { Uri = server.ConnectionString };
            var sendConfig = configGetter.GetEventSendConfig<T>();
            var connection = factory.CreateConnection();

            var channel = connection.CreateModel();
            if (Transaction.Current != null)
            {
                new RabbitMqResourceManager(connection, channel, Transaction.Current);
            }
            var body = sendConfig.Serializ(evt);
            channel.BasicPublish(sendConfig.Exchange(evt), sendConfig.RoutetingKey(evt), null, body);

            if (Transaction.Current == null)
            {
                channel.Dispose();
                connection.Dispose();
            }
        }
    }

    public interface IRabbitMqEventSendConfigGetter
    {
        IRabbitMqEventSendConfig<T> GetEventSendConfig<T>() where T : IEvent;
    }

    public interface IRabbitMqEventSendConfig<T> where T:IEvent
    {
        string Exchange(T evt);

        string RoutetingKey(T evt);

        byte[] Serializ(T evt);

        T Deserializ(string str);
    }
}