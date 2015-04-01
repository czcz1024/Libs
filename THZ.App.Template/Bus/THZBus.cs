namespace THZ.App.Template.Bus
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using Uninf.Bus;
    using Uninf.Bus.RabbitMq;
    using Uninf.Cache;

    public class THZBus:RabbitMqBus
    {
        private ICache cache;

        private IMessageSerializer s;

        private IRabbitServerConfig config;
        public THZBus(IRabbitServerConfig config, IMessageSerializer serializor, ICache cache)
            : base(config, serializor)
        {
            this.cache = cache;
            s = serializor;
            this.config = config;
        }

        protected override IEnumerable<IHandler<T>> GetHandlers<T>()
        {
            return DependencyResolver.Current.GetServices<IHandler<T>>();
        }

        protected override IResultHandler<T, TResult> GetResultHandler<T, TResult>()
        {
            return DependencyResolver.Current.GetService<IResultHandler<T, TResult>>();
        }

        protected override void AfterSendToQueue<T>(RabbitMQ.Client.IBasicProperties e, T msg)
        {
            var obj = new { 
                Id=e.MessageId,
                App=e.AppId,
                Body = s.Serialize(msg),
                MessageType=e.Type,
                RoutingKey=config.GetRoute(typeof(T))
            };
            cache.AddToList("AllMQSendMessages", s.Serialize(obj));
        }

        public static void StartAll()
        {
            //var ls = DependencyResolver.Current.GetServices<IAsyncHandler>();
            //foreach (var l in ls)
            //{
            //    l.Start();
            //}

            var ls = DependencyResolver.Current.GetServices<IHandler>().Where(x => x.Async());
            var starter = DependencyResolver.Current.GetService<IListener>();
            foreach (var l in ls)
            {
                starter.Start(l);
            }
        }
    }
}