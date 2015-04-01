// ***********************************************************************
// Assembly         : Uninf.Bus
// Author           : cz
// Created          : 01-07-2015
//
// Last Modified By : cz
// Last Modified On : 01-20-2015
// ***********************************************************************
// <copyright file="IHandler.cs" company="Uninf">
//     Copyright ©  2014
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.Bus
{
    /// <summary>
    /// IHandler 接口
    /// 泛型无返回值消息处理器接口
    /// </summary>
    /// <typeparam name="T">消息类型</typeparam>
    public interface IHandler<in T> : IHandler
    {
        /// <summary>
        /// 处理消息
        /// </summary>
        /// <param name="msg">The MSG.</param>
        void Handle(T msg);
    }

    /// <summary>
    /// IHandler 接口
    /// 消息处理器接口
    /// </summary>
    public interface IHandler
    {
        /// <summary>
        /// 处理消息
        /// </summary>
        /// <param name="msg">The MSG.</param>
        void Handle(string msg);
        /// <summary>
        /// 处理器执行顺序
        /// </summary>
        /// <returns>System.Int32.</returns>
        int Sort();

        /// <summary>
        /// 是否异步处理器
        /// </summary>
        /// <returns>异步返回<c>true</c>, 否则返回<c>false</c></returns>
        bool Async();

        /// <summary>
        /// 消息处理器对应接受消息的关键字
        /// </summary>
        /// <returns>System.String.</returns>
        string RoutingKey();
    }
}