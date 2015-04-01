// ***********************************************************************
// Assembly         : Uninf.Bus
// Author           : cz
// Created          : 01-07-2015
//
// Last Modified By : cz
// Last Modified On : 01-07-2015
// ***********************************************************************
// <copyright file="IMessageBus.cs" company="Uninf">
//     Copyright ©  2014
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.Bus
{
    /// <summary>
    /// IMessageBus 接口
    /// 消息总线
    /// </summary>
    public interface IMessageBus
    {
        /// <summary>
        /// 发送无返回值消息
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="msg">消息内容</param>
        void Send<T>(T msg);

        /// <summary>
        /// 发送有返回值消息，并返回返回值
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <typeparam name="TResult">返回值类型</typeparam>
        /// <param name="msg">消息内容</param>
        /// <returns>返回值</returns>
        TResult Call<T, TResult>(T msg);
    }
}