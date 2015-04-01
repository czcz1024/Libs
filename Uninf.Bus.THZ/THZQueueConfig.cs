// ***********************************************************************
// Assembly         : Uninf.Bus.THZ
// Author           : cz
// Created          : 01-08-2015
//
// Last Modified By : cz
// Last Modified On : 02-02-2015
// ***********************************************************************
// <copyright file="THZQueueConfig.cs" company="Uninf">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.Bus.THZ
{
    using System;

    using Uninf.Bus.RabbitMq;

    //public interface ITHZQueueConfig
    //{
    //    string GetExchangeName();

    //    string GetExchangeType();

    //    string GetRetryExchangeName();

    //    int GetRetryWaitSecond();

    //    string GetDeadExchangeName();

    //    string GetDeadQueueName();

    //    int GetRetryTimes();
    //}

    /// <summary>
    /// THZQueueConfig. 类
    /// </summary>
    public class THZQueueConfig : IRabbitServerConfig
    {
        /// <summary>
        /// Gets or sets the name of the exchange.
        /// </summary>
        /// <value>The name of the exchange.</value>
        public string ExchangeName { get; set; }
        /// <summary>
        /// Gets or sets the type of the exchange.
        /// </summary>
        /// <value>The type of the exchange.</value>
        public string ExchangeType { get; set; }

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the name of the retry exchange.
        /// </summary>
        /// <value>The name of the retry exchange.</value>
        public string RetryExchangeName { get; set; }

        /// <summary>
        /// Gets or sets the retry wait second.
        /// </summary>
        /// <value>The retry wait second.</value>
        public int RetryWaitSecond { get; set; }

        /// <summary>
        /// Gets or sets the name of the dead exchange.
        /// </summary>
        /// <value>The name of the dead exchange.</value>
        public string DeadExchangeName { get; set; }

        /// <summary>
        /// Gets or sets the name of the dead queue.
        /// </summary>
        /// <value>The name of the dead queue.</value>
        public string DeadQueueName { get; set; }

        /// <summary>
        /// Gets or sets the retry times.
        /// </summary>
        /// <value>The retry times.</value>
        public int RetryTimes { get; set; }

        /// <summary>
        /// Gets the exchange.
        /// </summary>
        /// <returns>System.String.</returns>
        public string GetExchange()
        {
            return ExchangeName;
        }

        /// <summary>
        /// Gets the route.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>System.String.</returns>
        public string GetRoute(Type type)
        {
            return type.FullName;
        }

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <returns>System.String.</returns>
        public string GetConnectionString()
        {
            return ConnectionString;
        }

        /// <summary>
        /// Gets the type of the exchange.
        /// </summary>
        /// <returns>System.String.</returns>
        public string GetExchangeType()
        {
            return ExchangeType;
        }

        /// <summary>
        /// Gets the dead exchange.
        /// </summary>
        /// <returns>System.String.</returns>
        public string GetDeadExchange()
        {
            return DeadExchangeName;
        }

        /// <summary>
        /// Gets the dead queue.
        /// </summary>
        /// <returns>System.String.</returns>
        public string GetDeadQueue()
        {
            return DeadQueueName;
        }

        /// <summary>
        /// Gets the retry exchange.
        /// </summary>
        /// <returns>System.String.</returns>
        public string GetRetryExchange()
        {
            return RetryExchangeName;
        }

        /// <summary>
        /// Gets the retry times.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public int GetRetryTimes()
        {
            return RetryTimes;
        }

        /// <summary>
        /// Gets the retry wait seconds.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public int GetRetryWaitSeconds()
        {
            return RetryWaitSecond;
        }

        /// <summary>
        /// Gets the connect time out.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public int GetConnectTimeOut()
        {
            return ConnectTimeOut;
        }

        /// <summary>
        /// Logs the handler error.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="handlerType">Type of the handler.</param>
        public virtual void LogHandlerError(Exception ex, Type handlerType)
        {
            
        }

        /// <summary>
        /// Logs the server error.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public virtual void LogServerError(Exception ex)
        {
            
        }

        /// <summary>
        /// Gets or sets a value indicating whether [automatic retry].
        /// </summary>
        /// <value><c>true</c> if [automatic retry]; otherwise, <c>false</c>.</value>
        public bool AutoRetry { get; set; }

        /// <summary>
        /// Automatics the retry when server down.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool AutoRetryWhenServerDown()
        {
            return AutoRetry;
        }

        /// <summary>
        /// Gets or sets the connect time out.
        /// </summary>
        /// <value>The connect time out.</value>
        public int ConnectTimeOut { get; set; }
    }
}