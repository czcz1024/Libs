// ***********************************************************************
// Assembly         : Uninf.Bus.RabbitMq
// Author           : cz
// Created          : 02-03-2015
//
// Last Modified By : cz
// Last Modified On : 02-03-2015
// ***********************************************************************
// <copyright file="RabbitMqReconnectManager.cs" company="Uninf">
//     Copyright ©  2014
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.Bus.RabbitMq
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using RabbitMQ.Client;

    /// <summary>
    /// RabbitMqReconnectManager. 类
    /// 重新连接服务管理类
    /// </summary>
    public class RabbitMqReconnectManager
    {
        /// <summary>
        /// The instance
        /// </summary>
        private static RabbitMqReconnectManager instance;

        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMqReconnectManager"/> class.
        /// </summary>
        protected RabbitMqReconnectManager()
        {
        }

        /// <summary>
        /// The locker
        /// </summary>
        private static readonly object locker = new object();
        /// <summary>
        /// Gets the current.
        /// </summary>
        /// <value>The current.</value>
        public static RabbitMqReconnectManager Current {
            get
            {
                if (instance == null)
                {
                    lock (locker)
                    {
                        if (instance == null)
                        {
                            instance = new RabbitMqReconnectManager();
                        }
                    }
                }
                return instance;
            }
        }



        /// <summary>
        /// The started
        /// </summary>
        private bool started = false;

        /// <summary>
        /// The lis
        /// </summary>
        private IListener lis;

        /// <summary>
        /// The handlers
        /// </summary>
        private BlockingCollection<IHandler> handlers = new BlockingCollection<IHandler>();

        /// <summary>
        /// Retries the connect.
        /// </summary>
        /// <param name="connstr">The connstr.</param>
        /// <param name="sleep">The sleep.</param>
        /// <param name="listener">The listener.</param>
        /// <param name="handler">The handler.</param>
        public void RetryConnect(string connstr,int sleep,IListener listener,IHandler handler)
        {
            if (!handlers.Contains(handler))
            {
                handlers.TryAdd(handler);
            }
            if (lis == null)
            {
                lis = listener;
            }
            if (!started)
            {
                new Task(() => { retry(connstr, sleep); }).Start();
                started = true;
            }
        }

        /// <summary>
        /// Retries the specified connstr.
        /// </summary>
        /// <param name="connstr">The connstr.</param>
        /// <param name="sleep">The sleep.</param>
        protected virtual void retry(string connstr,int sleep)
        {
            while (true)
            {
                try
                {
                    var factory = new ConnectionFactory { Uri = connstr };
                    using (var connection = factory.CreateConnection())
                    {
                        using (var channel = connection.CreateModel())
                        {
                            foreach (var h in handlers)
                            {
                                lis.Start(h);
                            }
                            handlers = new BlockingCollection<IHandler>();
                            started = false;
                            break;
                        }
                    }
                }
                catch(Exception ex)
                {
                    Thread.Sleep(sleep);
                }
            }
        }
    }
}