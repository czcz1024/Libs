// ***********************************************************************
// Assembly         : Uninf.Bus.RabbitMq
// Author           : cz
// Created          : 01-14-2015
//
// Last Modified By : cz
// Last Modified On : 02-13-2015
// ***********************************************************************
// <copyright file="RabbitMqListener.cs" company="Uninf">
//     Copyright ©  2014
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.Bus.RabbitMq
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;
    using RabbitMQ.Client.Exceptions;

    /// <summary>
    /// RabbitMqListener. 类
    /// 监听Rabbitmq实现基类
    /// </summary>
    public abstract class RabbitMqListener:IListener
    {
        /// <summary>
        /// The configuration
        /// </summary>
        protected IRabbitServerConfig config;

        /// <summary>
        /// The database
        /// </summary>
        protected IHandlerThreadStore db;




        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMqListener"/> class.
        /// </summary>
        /// <param name="db">The database.</param>
        /// <param name="config">The configuration.</param>
        protected RabbitMqListener(IHandlerThreadStore db, IRabbitServerConfig config)
        {
            this.db = db;
            this.config = config;
        }

        /// <summary>
        /// Starts the specified handler.
        /// </summary>
        /// <param name="handler">The handler.</param>
        public void Start(IHandler handler)
        {
            if (!handler.Async()) return;
            var c = new CancellationTokenSource();
            //var tf = new TaskFactory();
            //tf.StartNew(() => StartListen(handler,c), c.Token);
            new Task(() => StartListen(handler, c), c.Token).Start();
        }

        /// <summary>
        /// Stops the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="threadId">The thread identifier.</param>
        public void Stop(Type type, int threadId)
        {
            var c=db.Get(type, threadId) as IModel;
            if (c != null)
            {
                c.BasicCancel(threadId.ToString());
            }
        }

        /// <summary>
        /// Stops all.
        /// </summary>
        /// <param name="type">The type.</param>
        public void StopAll(Type type)
        {
            var list = db.GetAll()[type].Select(x=>new{threaid=x.Item1,channel=x.Item2}).ToList();
            foreach (var item in list)
            {
                var c = item.channel as IModel;
                if (c != null)
                {
                    c.BasicCancel(item.threaid.ToString());
                }
            }
        }

        /// <summary>
        /// Deletes the asynchronous handler.
        /// </summary>
        /// <param name="handler">The handler.</param>
        public void DeleteAsyncHandler(IHandler handler)
        {
            var factory = new ConnectionFactory { Uri = config.GetConnectionString() };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueUnbind(QueueName(handler), config.GetExchange(), RouteKeyOrTopic(handler), null);
                }
            }

            new Task(() => { DeleteQueueWhenAllDone(handler); }).Start();
        }

        /// <summary>
        /// Deletes the queue when all done.
        /// </summary>
        /// <param name="handler">The handler.</param>
        protected void DeleteQueueWhenAllDone(IHandler handler)
        {
            var factory = new ConnectionFactory { Uri = config.GetConnectionString() };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    while (true)
                    {
                        var workq = channel.QueueDeclarePassive(QueueName(handler));
                        var retryq = channel.QueueDeclarePassive(RetryQueueName(handler));
                        var workCnt = workq.MessageCount;
                        var retryCnt = retryq.MessageCount;
                        if (workCnt == 0 && retryCnt == 0)
                        {
                            var list = db.GetAll()[handler.GetType()].Select(x => new { threaid = x.Item1, channel = x.Item2 }).ToList();
                            foreach (var item in list)
                            {
                                var c = item.channel as IModel;
                                if (c != null)
                                {
                                    c.BasicCancel(item.threaid.ToString());
                                }
                            }
                            channel.QueueDelete(QueueName(handler), true, true);
                            channel.QueueDelete(RetryQueueName(handler), true, true);
                            break;
                        }
                        Thread.Sleep(1000);
                    }
                }
            }
        }
        /// <summary>
        /// The last error fix
        /// </summary>
        bool lastErrFix = true;

        /// <summary>
        /// Gets the thread identifier.
        /// </summary>
        /// <returns>System.Int32.</returns>
        protected virtual int GetThreadId()
        {
            return Thread.CurrentThread.ManagedThreadId;
        }

        /// <summary>
        /// Starts the listen.
        /// </summary>
        /// <param name="handler">The handler.</param>
        /// <param name="c">The c.</param>
        protected virtual void StartListen(IHandler handler,CancellationTokenSource c)
        {
            var threadid = GetThreadId();
            var factory = new ConnectionFactory { Uri = config.GetConnectionString() };
            var qname = QueueName(handler);
            try
            {
                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        lastErrFix = true;
                        SaveListener(threadid, channel, handler);
                        channel.ExchangeDeclare(config.GetDeadExchange(), "direct", true);
                        channel.QueueDeclare(config.GetDeadQueue(), true, false, false, null);
                        channel.QueueBind(config.GetDeadQueue(), config.GetDeadExchange(), "");

                        channel.ExchangeDeclare(config.GetRetryExchange(), "direct", true);
                        var retryQueueArgs = new Dictionary<string, object>
                                                 {
                                                     {
                                                         "x-dead-letter-exchange",
                                                         config.GetExchange()
                                                     },
                                                     {
                                                         "x-message-ttl",
                                                         RetryWaitSecond() * 1000
                                                     }
                                                 };
                        var retryQ = channel.QueueDeclare(RetryQueueName(handler), true, false, false, retryQueueArgs);
                        channel.QueueBind(
                            RetryQueueName(handler),
                            config.GetRetryExchange(),
                            RetryRouteKeyOrTopic(handler));

                        channel.ExchangeDeclare(config.GetExchange(), config.GetExchangeType(), true);
                        var workQ = channel.QueueDeclare(qname, true, false, false, null);
                        channel.QueueBind(qname, config.GetExchange(), RouteKeyOrTopic(handler));
                        channel.QueueBind(qname, config.GetExchange(), RetryRouteKeyOrTopic(handler));

                        channel.BasicQos(0, 1, false);
                        var consumer = new QueueingBasicConsumer(channel);
                        channel.BasicConsume(qname, false, threadid.ToString(), consumer);
                        consumer.ConsumerCancelled += (sender, args) => { DeleteListener(threadid, handler); };

                        while (true)
                        {
                            try
                            {
                                BasicDeliverEventArgs e = consumer.Queue.Dequeue();
                                var message = Encoding.UTF8.GetString(e.Body);
                                if (IsSkipThisMessage(e.BasicProperties, e.RoutingKey, message))
                                {
                                    channel.BasicAck(e.DeliveryTag, false);
                                    continue;
                                }
                                try
                                {
                                    handler.Handle(message);
                                    SaveDoneMessage(e.BasicProperties, e.RoutingKey, message);
                                }
                                catch (Exception ex)
                                {
                                    int retryTimes = GetRetryTimes(e.BasicProperties, message, ex);

                                    if (retryTimes < RetryTimes())
                                    {
                                        AddRetryTime(e.BasicProperties, message, ex);
                                        channel.BasicPublish(
                                            config.GetRetryExchange(),
                                            RetryRouteKeyOrTopic(handler),
                                            e.BasicProperties,
                                            e.Body);
                                    }
                                    else
                                    {
                                        channel.BasicPublish(config.GetDeadExchange(), "", e.BasicProperties, e.Body);
                                    }

                                    config.LogHandlerError(ex, handler.GetType());
                                }
                                channel.BasicAck(e.DeliveryTag, false);
                            }
                            catch (EndOfStreamException ex)
                            {
                                break;
                            }
                        }
                        //DeleteListener(threadid,handler);
                    }
                }
            }
            catch (BrokerUnreachableException ex)
            {
                if (lastErrFix)
                {
                    config.LogServerError(ex);
                    lastErrFix = false;
                }
                DeleteListener(threadid, handler);
                if (config.AutoRetryWhenServerDown())
                {
                    //Thread.Sleep(ReConnectWait());
                    //StartListen(handler, c);
                    RabbitMqReconnectManager.Current.RetryConnect(
                        config.GetConnectionString(),
                        ReConnectWait(),
                        this,
                        handler);
                }
            }
            catch (IOException ex)
            {
                if (lastErrFix)
                {
                    config.LogServerError(ex);
                    lastErrFix = false;
                }
                DeleteListener(threadid, handler);
                if (config.AutoRetryWhenServerDown())
                {
                    //Thread.Sleep(ReConnectWait());
                    //StartListen(handler, c);
                    RabbitMqReconnectManager.Current.RetryConnect(
                        config.GetConnectionString(),
                        ReConnectWait(),
                        this,
                        handler);
                }
            }
        }

        /// <summary>
        /// Res the connect wait.
        /// </summary>
        /// <returns>System.Int32.</returns>
        protected virtual int ReConnectWait()
        {
            return 5000;
        }

        /// <summary>
        /// Saves the done message.
        /// </summary>
        /// <param name="basicProperties">The basic properties.</param>
        /// <param name="routingKey">The routing key.</param>
        /// <param name="message">The message.</param>
        protected abstract void SaveDoneMessage(IBasicProperties basicProperties, string routingKey, string message);

        /// <summary>
        /// Gets the retry times.
        /// </summary>
        /// <param name="e">The e.</param>
        /// <param name="message">The message.</param>
        /// <param name="exp">The exp.</param>
        /// <returns>System.Int32.</returns>
        protected int GetRetryTimes(IBasicProperties e, string message, Exception exp)
        {
            if (e.Headers == null)
            {
                e.Headers = new Dictionary<string, object>();
                return 0;
            }
            if (e.Headers.ContainsKey("retry"))
            {
                var rt = Convert.ToInt32(e.Headers["retry"]);
                return rt;
            }
            else
            {
                e.Headers.Add("retry", 0);
            }
            return 0;
        }

        /// <summary>
        /// Adds the retry time.
        /// </summary>
        /// <param name="e">The e.</param>
        /// <param name="message">The message.</param>
        /// <param name="exp">The exp.</param>
        protected void AddRetryTime(IBasicProperties e, string message, Exception exp)
        {
            var retry = 0;
            if (e.Headers.ContainsKey("retry"))
            {
                retry = Convert.ToInt32(e.Headers["retry"]);
            }
            var err = "";
            if (e.Headers.ContainsKey("errmessage"))
            {
                err = Encoding.UTF8.GetString(e.Headers["errmessage"] as byte[]);
            }
            err += "," + exp.Message;
            e.Headers["retry"] = retry + 1;
            e.Headers["errmessage"] = err;
        }

        /// <summary>
        /// Saves the listener.
        /// </summary>
        /// <param name="threadid">The threadid.</param>
        /// <param name="c">The c.</param>
        /// <param name="handler">The handler.</param>
        protected void SaveListener(int threadid, object c,IHandler handler)
        {
            db.Add(handler.GetType(), threadid, c);
        }

        /// <summary>
        /// Deletes the listener.
        /// </summary>
        /// <param name="threadid">The threadid.</param>
        /// <param name="handler">The handler.</param>
        protected void DeleteListener(int threadid, IHandler handler)
        {
            db.Delete(handler.GetType(), threadid);
        }

        /// <summary>
        /// Queues the name.
        /// </summary>
        /// <param name="handler">The handler.</param>
        /// <returns>System.String.</returns>
        protected abstract string QueueName(IHandler handler);

        /// <summary>
        /// Retries the route key or topic.
        /// </summary>
        /// <param name="handler">The handler.</param>
        /// <returns>System.String.</returns>
        protected virtual string RetryRouteKeyOrTopic(IHandler handler)
        {
            return RouteKeyOrTopic(handler) + "-" + handler.GetType().FullName;
        }

        /// <summary>
        /// Routes the key or topic.
        /// </summary>
        /// <param name="handler">The handler.</param>
        /// <returns>System.String.</returns>
        protected abstract string RouteKeyOrTopic(IHandler handler);

        /// <summary>
        /// Retries the name of the queue.
        /// </summary>
        /// <param name="handler">The handler.</param>
        /// <returns>System.String.</returns>
        protected string RetryQueueName(IHandler handler)
        {
            return QueueName(handler) + "-Retry";
        }

        /// <summary>
        /// Retries the wait second.
        /// </summary>
        /// <returns>System.Int32.</returns>
        protected abstract int RetryWaitSecond();

        /// <summary>
        /// Retries the times.
        /// </summary>
        /// <returns>System.Int32.</returns>
        protected abstract int RetryTimes();

        /// <summary>
        /// Determines whether [is skip this message] [the specified e].
        /// </summary>
        /// <param name="e">The e.</param>
        /// <param name="routingKey">The routing key.</param>
        /// <param name="msg">The MSG.</param>
        /// <returns><c>true</c> if [is skip this message] [the specified e]; otherwise, <c>false</c>.</returns>
        protected abstract bool IsSkipThisMessage(IBasicProperties e, string routingKey, string msg);
    }
}