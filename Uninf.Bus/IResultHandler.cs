// ***********************************************************************
// Assembly         : Uninf.Bus
// Author           : cz
// Created          : 01-07-2015
//
// Last Modified By : cz
// Last Modified On : 01-20-2015
// ***********************************************************************
// <copyright file="IResultHandler.cs" company="Uninf">
//     Copyright ©  2014
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.Bus
{
    /// <summary>
    /// IResultHandler 接口
    /// 有返回值消息处理器泛型接口
    /// </summary>
    /// <typeparam name="T">消息类型</typeparam>
    /// <typeparam name="TResult">返回值类型</typeparam>
    public interface IResultHandler<in T, out TResult> : IHandler
    {
        /// <summary>
        /// 处理消息
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <returns>TResult.</returns>
        TResult Handle(T msg);
    }
}