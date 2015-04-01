// ***********************************************************************
// Assembly         : Uninf.Bus.RabbitMq
// Author           : cz
// Created          : 01-09-2015
//
// Last Modified By : cz
// Last Modified On : 01-29-2015
// ***********************************************************************
// <copyright file="RabbitMqBus.cs" company="Uninf">
//     Copyright ©  2014
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.Bus.RabbitMq
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Transactions;

    using RabbitMQ.Client;

    /// <summary>
    /// RabbitMqBus. 类
    /// 基于rabbitmq的总线基类
    /// </summary>
    public abstract class RabbitMqBus:MessageBusBase
    {
        /// <summary>
        /// The configuration
        /// </summary>
        private IRabbitServerConfig config;

        /// <summary>
        /// The serializor
        /// </summary>
        private IMessageSerializer serializor;
        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMqBus"/> class.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <param name="serializor">The serializor.</param>
        protected RabbitMqBus(IRabbitServerConfig config, IMessageSerializer serializor)
        {
            this.config = config;
            this.serializor = serializor;
        }

        /// <summary>
        /// Sends to queue.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg">The MSG.</param>
        protected override void SendToQueue<T>(T msg)
        {
            
            var factory = new ConnectionFactory { 
                Uri = config.GetConnectionString() ,
                RequestedConnectionTimeout=config.GetConnectTimeOut(),
            };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            try
            {
                if (Transaction.Current != null)
                {
                    var trans = new RabbitMqResourceManager(connection, channel, Transaction.Current);
                }
                channel.ExchangeDeclare(config.GetExchange(), config.GetExchangeType(),true);
                var body = Encoding.UTF8.GetBytes(serializor.Serialize(msg));
                var pro = channel.CreateBasicProperties();
                pro.MessageId = Guid.NewGuid().ToString();
                pro.AppId = this.GetType().FullName;
                pro.Type = typeof(T).FullName;
                pro.DeliveryMode = 2;
                this.BeforeSendToQueue(pro, msg);
                channel.BasicPublish(config.GetExchange(), config.GetRoute(typeof(T)), pro, body);
                this.AfterSendToQueue(pro, msg);
            }
            catch
            {
                throw;
            }
            finally
            {
                if (Transaction.Current == null)
                {
                    channel.Dispose();
                    connection.Dispose();
                }
            }
        }

        /// <summary>
        /// Befores the send to queue.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e">The e.</param>
        /// <param name="msg">The MSG.</param>
        protected virtual void BeforeSendToQueue<T>(IBasicProperties e, T msg)
        {
        }

        /// <summary>
        /// Afters the send to queue.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e">The e.</param>
        /// <param name="msg">The MSG.</param>
        protected virtual void AfterSendToQueue<T>(IBasicProperties e, T msg)
        {
        }

        /// <summary>
        /// Befores the send.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg">The MSG.</param>
        protected virtual void BeforeSend<T>(T msg)
        {
        }

        /// <summary>
        /// Afters the send.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg">The MSG.</param>
        protected virtual void AfterSend<T>(T msg)
        {
        }

        /// <summary>
        /// Sends the specified MSG.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg">The MSG.</param>
        public override void Send<T>(T msg)
        {
            BeforeSend<T>(msg);
            base.Send<T>(msg);
            AfterSend(msg);
        }

        /// <summary>
        /// Calls the specified MSG.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult">The type of the t result.</typeparam>
        /// <param name="msg">The MSG.</param>
        /// <returns>TResult.</returns>
        public override TResult Call<T, TResult>(T msg)
        {
            BeforeCall<T,TResult>(msg);
            var result=base.Call<T, TResult>(msg);
            AfterCall(msg, result);
            return result;
        }

        /// <summary>
        /// Befores the call.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult">The type of the t result.</typeparam>
        /// <param name="msg">The MSG.</param>
        protected virtual void BeforeCall<T, TResult>(T msg)
        {
        }

        /// <summary>
        /// Afters the call.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult">The type of the t result.</typeparam>
        /// <param name="msg">The MSG.</param>
        /// <param name="result">The result.</param>
        protected virtual void AfterCall<T, TResult>(T msg, TResult result)
        {
        }

    }
}