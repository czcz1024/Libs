// ***********************************************************************
// Assembly         : Uninf.Bus
// Author           : cz
// Created          : 01-14-2015
//
// Last Modified By : cz
// Last Modified On : 01-27-2015
// ***********************************************************************
// <copyright file="IListener.cs" company="Uninf">
//     Copyright ©  2014
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.Bus
{
    using System;

    /// <summary>
    /// IListener 接口
    /// 消息队列服务监听接口
    /// </summary>
    public interface IListener
    {
        /// <summary>
        /// 开始一个线程监听
        /// </summary>
        /// <param name="handler">处理器</param>
        void Start(IHandler handler);

        /// <summary>
        /// 停止一个监听线程
        /// </summary>
        /// <param name="type">处理器类型</param>
        /// <param name="threadId">线程id</param>
        void Stop(Type type, int threadId);

        /// <summary>
        /// 全部停止某种处理器的监听线程
        /// </summary>
        /// <param name="type">处理器类型</param>
        void StopAll(Type type);

        /// <summary>
        /// 异步转同步
        /// </summary>
        /// <param name="handler">处理器</param>
        void DeleteAsyncHandler(IHandler handler);
    }
}