// ***********************************************************************
// Assembly         : Uninf.Bus.RabbitMq
// Author           : cz
// Created          : 01-09-2015
//
// Last Modified By : cz
// Last Modified On : 02-02-2015
// ***********************************************************************
// <copyright file="IRabbitServerConfig.cs" company="Uninf">
//     Copyright ©  2014
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.Bus.RabbitMq
{
    using System;

    /// <summary>
    /// IRabbitServerConfig 接口
    /// </summary>
    public interface IRabbitServerConfig
    {
        /// <summary>
        /// Gets the exchange.
        /// </summary>
        /// <returns>System.String.</returns>
        string GetExchange();

        /// <summary>
        /// Gets the route.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>System.String.</returns>
        string GetRoute(Type type);

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <returns>System.String.</returns>
        string GetConnectionString();

        /// <summary>
        /// Gets the type of the exchange.
        /// </summary>
        /// <returns>System.String.</returns>
        string GetExchangeType();

        /// <summary>
        /// Gets the dead exchange.
        /// </summary>
        /// <returns>System.String.</returns>
        string GetDeadExchange();

        /// <summary>
        /// Gets the dead queue.
        /// </summary>
        /// <returns>System.String.</returns>
        string GetDeadQueue();

        /// <summary>
        /// Gets the retry exchange.
        /// </summary>
        /// <returns>System.String.</returns>
        string GetRetryExchange();

        /// <summary>
        /// Gets the retry times.
        /// </summary>
        /// <returns>System.Int32.</returns>
        int GetRetryTimes();

        /// <summary>
        /// Gets the retry wait seconds.
        /// </summary>
        /// <returns>System.Int32.</returns>
        int GetRetryWaitSeconds();

        /// <summary>
        /// Gets the connect time out.
        /// </summary>
        /// <returns>System.Int32.</returns>
        int GetConnectTimeOut();

        /// <summary>
        /// Logs the handler error.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="handlerType">Type of the handler.</param>
        void LogHandlerError(Exception ex, Type handlerType);

        /// <summary>
        /// Logs the server error.
        /// </summary>
        /// <param name="ex">The ex.</param>
        void LogServerError(Exception ex);

        /// <summary>
        /// Automatics the retry when server down.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool AutoRetryWhenServerDown();
    }
}