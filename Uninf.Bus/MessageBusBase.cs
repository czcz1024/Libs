// ***********************************************************************
// Assembly         : Uninf.Bus
// Author           : cz
// Created          : 01-07-2015
//
// Last Modified By : cz
// Last Modified On : 01-29-2015
// ***********************************************************************
// <copyright file="MessageBusBase.cs" company="Uninf">
//     Copyright ©  2014
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.Bus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// MessageBusBase. 类
    /// 总线基类
    /// </summary>
    public abstract class MessageBusBase:IMessageBus
    {
        /// <summary>
        /// 发送无返回值消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg">The MSG.</param>
        public virtual void Send<T>(T msg)
        {
            BeforeSendToQueue<T>(msg);
            SendToQueue(msg);
            SendToQueueSuccess<T>(msg);
            var handlers = this.GetHandlers<T>();
            foreach (var handler in handlers.Where(x=>!x.Async()).OrderBy(x => x.Sort()))
            {
                handler.Handle(msg);
            }

            HandlerProcessSuccess<T>(msg);
        }

        /// <summary>
        /// 无消息处理成功
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg">The MSG.</param>
        protected virtual void HandlerProcessSuccess<T>(T msg)
        {
            
        }

        /// <summary>
        /// 发送到消息队列成功
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg">The MSG.</param>
        protected virtual void SendToQueueSuccess<T>(T msg)
        {
            
        }

        /// <summary>
        /// 发送到消息队列之前
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg">The MSG.</param>
        protected virtual void BeforeSendToQueue<T>(T msg)
        {
            
        }

        /// <summary>
        /// 发送到消息队列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg">The MSG.</param>
        protected abstract void SendToQueue<T>(T msg);

        /// <summary>
        /// 发送有返回值消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult">The type of the t result.</typeparam>
        /// <param name="msg">The MSG.</param>
        /// <returns>TResult.</returns>
        public virtual TResult Call<T, TResult>(T msg)
        {
            var handler = this.GetResultHandler<T, TResult>();
            return handler.Handle(msg);
        }

        /// <summary>
        /// 获取某无返回值消息的所有处理器
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <returns>IEnumerable&lt;IHandler&lt;T&gt;&gt;.</returns>
        protected abstract IEnumerable<IHandler<T>> GetHandlers<T>();

        /// <summary>
        /// 获取一个有返回值消息的处理器
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <typeparam name="TResult">返回值类型</typeparam>
        /// <returns>IResultHandler&lt;T, TResult&gt;.</returns>
        protected abstract IResultHandler<T, TResult> GetResultHandler<T, TResult>();
    }
}