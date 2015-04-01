namespace Uninf.Bus.RabbitMq
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Transactions;

    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;

    //public abstract class RabbitMqHandlerBase<T> :RabbitMqHandlerBase, IAsyncHandler<T>
    //{
    //    protected RabbitMqHandlerBase(IMessageServerConfig config, IHandlerThreadStore db)
    //        : base(config, db)
    //    {
    //    }

    //    public abstract void Handle(T msg);
    //    public virtual void Start()
    //    {
    //        var c = new CancellationTokenSource();
    //        var tf = new TaskFactory();
    //        tf.StartNew(() => StartListen(c), c.Token);
    //    }

    //    public override void Handle(string msg)
    //    {
    //        //try
    //        //{
    //            var obj=Deserialize(msg);
    //            Handle(obj);
    //        //}
    //        //catch
    //        //{
    //        //    //throw new Exception("消息不是" + typeof(T));
    //        //}
    //    }

    //    public abstract T Deserialize(string msg);

    //    protected override string QueueName()
    //    {
    //        return "THZ-" + this.GetType().FullName + "-Queue";
    //    }

    //    protected override string RouteKeyOrTopic()
    //    {
    //        return config.GetRoute<T>();
    //    }

    //    #region del
    //    //protected virtual void StartListen(CancellationTokenSource c)
    //    //{
    //    //    var threadid = Thread.CurrentThread.ManagedThreadId;
    //    //    SaveListener(threadid, c);
    //    //    var factory = new ConnectionFactory { Uri = ConnectionString() };
    //    //    using (var connection = factory.CreateConnection())
    //    //    {
    //    //        using (var channel = connection.CreateModel())
    //    //        {

    //    //            channel.ExchangeDeclare(DeadExchangeName(), "direct");
    //    //            channel.QueueDeclare(DeadQueueName(), true, false, false, null);
    //    //            channel.QueueBind(DeadQueueName(), DeadExchangeName(), "");

    //    //            channel.ExchangeDeclare(RetryExchangeName(), "direct");
    //    //            var retryQueueArgs = new Dictionary<string, object> {
    //    //                { "x-dead-letter-exchange", ExchangeName() },
    //    //                { "x-message-ttl", RetryWaitSecond()*1000 }
    //    //            };
    //    //            channel.QueueDeclare(RetryQueueName(), true, false, false, retryQueueArgs);
    //    //            channel.QueueBind(RetryQueueName(), RetryExchangeName(), RetryRouteKeyOrTopic());

    //    //            channel.ExchangeDeclare(ExchangeName(), ExchangeType());
    //    //            channel.QueueDeclare(RoutingKey(), true, false, false, null);
    //    //            channel.QueueBind(RoutingKey(), ExchangeName(), RouteKeyOrTopic());
    //    //            channel.QueueBind(RoutingKey(), ExchangeName(), RetryRouteKeyOrTopic());

    //    //            var consumer = new QueueingBasicConsumer(channel);
    //    //            channel.BasicConsume(RoutingKey(), false, consumer);
    //    //            while (true)
    //    //            {
    //    //                BasicDeliverEventArgs e;
    //    //                consumer.Queue.Dequeue(1000, out e);
    //    //                if (c.IsCancellationRequested || c.Token.IsCancellationRequested)
    //    //                {
    //    //                    break;
    //    //                }
    //    //                if (e == null) continue;
    //    //                if (e.Body == null) continue;

    //    //                var message = Encoding.UTF8.GetString(e.Body);
    //    //                try
    //    //                {
    //    //                    Handle(message);
    //    //                }
    //    //                catch (Exception ex)
    //    //                {
    //    //                    int retryTimes;
    //    //                    IBasicProperties pro;
    //    //                    var newBody = Encoding.UTF8.GetBytes(GetNewMessageBody(e, message, ex, out retryTimes,out pro));
    //    //                    if (retryTimes < RetryTimes())
    //    //                    {

    //    //                        channel.BasicPublish(RetryExchangeName(), RetryRouteKeyOrTopic(), pro, newBody);
    //    //                    }
    //    //                    else
    //    //                    {
    //    //                        channel.BasicPublish(DeadExchangeName(), "", pro, newBody);
    //    //                    }
    //    //                }
    //    //                channel.BasicAck(e.DeliveryTag, false);
    //    //            }
    //    //            DeleteListener(threadid);
    //    //        }
    //    //    }
    //    //}
    //    #endregion
    //}

    //public abstract class RabbitMqHandlerBase : IAsyncHandler
    //{
    //    protected IMessageServerConfig config;

    //    protected IHandlerThreadStore db;

    //    protected RabbitMqHandlerBase(IMessageServerConfig config, IHandlerThreadStore db)
    //    {
    //        this.config = config;
    //        this.db = db;
    //    }

    //    public void Start()
    //    {
    //        var c = new CancellationTokenSource();
    //        var tf = new TaskFactory();
    //        tf.StartNew(() => StartListen(c), c.Token);
    //    }

    //    public abstract void Handle(string msg);

    //    protected virtual void StartListen(CancellationTokenSource c)
    //    {
    //        var threadid = Thread.CurrentThread.ManagedThreadId;
    //        SaveListener(threadid, c);
    //        var factory = new ConnectionFactory { Uri = config.GetConnectionString() };
    //        using (var connection = factory.CreateConnection())
    //        {
    //            using (var channel = connection.CreateModel())
    //            {
    //                channel.ExchangeDeclare(config.GetDeadExchange(), "direct",true);
    //                channel.QueueDeclare(config.GetDeadQueue(), true, false, false, null);
    //                channel.QueueBind(config.GetDeadQueue(),config.GetDeadExchange(), "");

    //                channel.ExchangeDeclare(config.GetRetryExchange(), "direct", true);
    //                var retryQueueArgs = new Dictionary<string, object> {
    //                    { "x-dead-letter-exchange", config.GetExchange() },
    //                    { "x-message-ttl", RetryWaitSecond()*1000 }
    //                };
    //                channel.QueueDeclare(RetryQueueName(), true, false, false, retryQueueArgs);
    //                channel.QueueBind(RetryQueueName(), config.GetRetryExchange(), RetryRouteKeyOrTopic());

    //                channel.ExchangeDeclare(config.GetExchange(), config.GetExchangeType(), true);
    //                channel.QueueDeclare(QueueName(), true, false, false, null);
    //                channel.QueueBind(QueueName(), config.GetExchange(), RouteKeyOrTopic());
    //                channel.QueueBind(QueueName(), config.GetExchange(), RetryRouteKeyOrTopic());

    //                var consumer = new QueueingBasicConsumer(channel);
    //                channel.BasicConsume(QueueName(), false, consumer);
    //                while (true)
    //                {
    //                    BasicDeliverEventArgs e;
    //                    try
    //                    {
    //                        consumer.Queue.Dequeue(1000, out e);
    //                    }
    //                    catch
    //                    {
    //                        break;
    //                    }
    //                    if (c.IsCancellationRequested || c.Token.IsCancellationRequested)
    //                    {
    //                        break;
    //                    }
    //                    if (e == null) continue;
    //                    if (e.Body == null) continue;

    //                    var message = Encoding.UTF8.GetString(e.Body);
    //                    if (IsSkipThisMessage(e.BasicProperties, e.RoutingKey, message))
    //                    {
    //                        continue;
    //                    }
    //                    try
    //                    {
    //                        this.Handle(message);
    //                        SaveDoneMessage(e.BasicProperties, e.RoutingKey, message);
    //                    }
    //                    catch (Exception ex)
    //                    {
    //                        int retryTimes=GetRetryTimes(e.BasicProperties,message,ex);
                            
    //                        if (retryTimes < RetryTimes())
    //                        {
    //                            AddRetryTime(e.BasicProperties, message, ex);
    //                            channel.BasicPublish(config.GetRetryExchange(), RetryRouteKeyOrTopic(), e.BasicProperties, e.Body);
    //                        }
    //                        else
    //                        {
    //                            channel.BasicPublish(config.GetDeadExchange(), "", e.BasicProperties, e.Body);
    //                        }
    //                    }
    //                    channel.BasicAck(e.DeliveryTag, false);
    //                }
    //                DeleteListener(threadid);
    //            }
    //        }
    //    }

    //    protected abstract void SaveDoneMessage(IBasicProperties basicProperties, string routingKey, string message);

    //    //protected abstract string GetNewMessageBody(BasicDeliverEventArgs basicDeliverEventArgs, object message, Exception exception, out int retryTimes, out IBasicProperties pro);
    //    protected int GetRetryTimes(IBasicProperties e, string message, Exception exp)
    //    {
    //        if (e.Headers == null)
    //        {
    //            e.Headers = new Dictionary<string, object>();
    //            return 0;
    //        }
    //        if (e.Headers.ContainsKey("retry"))
    //        {
    //            var rt = Convert.ToInt32(e.Headers["retry"]);
    //            return rt;
    //        }
    //        else
    //        {
    //            e.Headers.Add("retry", 0);
    //        }
    //        return 0;
    //    }

    //    protected void AddRetryTime(IBasicProperties e, string message, Exception exp)
    //    {
    //        var retry = 0;
    //        if (e.Headers.ContainsKey("retry"))
    //        {
    //            retry = Convert.ToInt32(e.Headers["retry"]);
    //        }
    //        var err = "";
    //        if (e.Headers.ContainsKey("errmessage"))
    //        {
    //            err =Encoding.UTF8.GetString(e.Headers["errmessage"] as byte[]);
    //        }
    //        err += "," + exp.Message;
    //        e.Headers["retry"] = retry + 1;
    //        e.Headers["errmessage"] = err;
    //    }

    //    protected void Stop(int threadid)
    //    {
    //        var c = db.Get(this.GetType(), threadid);
    //        if (c != null)
    //        {
    //            c.Cancel();
    //        }
    //    }

    //    protected void SaveListener(int threadid, CancellationTokenSource c)
    //    {
    //        db.Add(this.GetType(), threadid, c);
    //    }

    //    protected void DeleteListener(int threadid)
    //    {
    //        db.Delete(this.GetType(), threadid);
    //    }

    //    protected abstract string QueueName();

    //    protected virtual string RetryRouteKeyOrTopic()
    //    {
    //        return RouteKeyOrTopic() + "-" + this.GetType().FullName;
    //    }

    //    protected abstract string RouteKeyOrTopic();

    //    protected string RetryQueueName()
    //    {
    //        return QueueName() + "-Retry";
    //    }

    //    protected abstract int RetryWaitSecond();

    //    protected abstract int RetryTimes();

    //    protected abstract bool IsSkipThisMessage(IBasicProperties e,string routingKey, string msg);
    //}
}