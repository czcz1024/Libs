// ***********************************************************************
// Assembly         : Uninf.Bus.THZ
// Author           : cz
// Created          : 01-14-2015
//
// Last Modified By : cz
// Last Modified On : 01-27-2015
// ***********************************************************************
// <copyright file="THZListener.cs" company="Uninf">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.Bus.THZ
{
    using RabbitMQ.Client;

    using Uninf.Bus.RabbitMq;

    /// <summary>
    /// THZListener. 类
    /// 为thz项目配置过的线程监听类
    /// </summary>
    public class THZListener : RabbitMqListener
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="THZListener"/> class.
        /// </summary>
        /// <param name="db">The database.</param>
        /// <param name="config">The configuration.</param>
        public THZListener(IHandlerThreadStore db, IRabbitServerConfig config)
            : base(db, config)
        {
        }

        /// <summary>
        /// 保存处理成功的消息
        /// </summary>
        /// <param name="basicProperties">The basic properties.</param>
        /// <param name="routingKey">The routing key.</param>
        /// <param name="message">The message.</param>
        protected override void SaveDoneMessage(IBasicProperties basicProperties, string routingKey, string message)
        {
            
        }

        /// <summary>
        /// 队列名称
        /// </summary>
        /// <param name="handler">The handler.</param>
        /// <returns>System.String.</returns>
        protected override string QueueName(IHandler handler)
        {
            return "THZ-" + handler.GetType().FullName + "-Queue";
        }

        /// <summary>
        /// 关键字
        /// </summary>
        /// <param name="handler">The handler.</param>
        /// <returns>System.String.</returns>
        protected override string RouteKeyOrTopic(IHandler handler)
        {
            return handler.RoutingKey();
        }

        /// <summary>
        /// 重试间隔时间
        /// </summary>
        /// <returns>System.Int32.</returns>
        protected override int RetryWaitSecond()
        {
            return config.GetRetryWaitSeconds();
        }

        /// <summary>
        /// 重试次数
        /// </summary>
        /// <returns>System.Int32.</returns>
        protected override int RetryTimes()
        {
            return config.GetRetryTimes();
        }

        /// <summary>
        /// 判断是否跳过消息
        /// </summary>
        /// <param name="e">The e.</param>
        /// <param name="routingKey">The routing key.</param>
        /// <param name="msg">The MSG.</param>
        /// <returns><c>true</c> if [is skip this message] [the specified e]; otherwise, <c>false</c>.</returns>
        protected override bool IsSkipThisMessage(IBasicProperties e, string routingKey, string msg)
        {
            return false;
        }
    }
}