// ***********************************************************************
// Assembly         : Uninf.Bus
// Author           : cz
// Created          : 01-20-2015
//
// Last Modified By : cz
// Last Modified On : 01-20-2015
// ***********************************************************************
// <copyright file="ResultHandlerBase.cs" company="Uninf">
//     Copyright ©  2014
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.Bus
{
    using System;

    /// <summary>
    /// ResultHandlerBase. 类
    /// 有返回值消息处理器基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult">The type of the t result.</typeparam>
    public abstract class ResultHandlerBase<T,TResult>:IResultHandler<T,TResult>
    {
        /// <summary>
        /// 处理消息
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <returns>TResult.</returns>
        public abstract TResult Handle(T msg);

        /// <summary>
        /// Handles the specified MSG.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <exception cref="System.Exception">带有返回值的处理器，不应该调用此方法</exception>
        public void Handle(string msg)
        {
            throw new Exception("带有返回值的处理器，不应该调用此方法");
        }

        /// <summary>
        /// Sorts this instance.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public virtual int Sort()
        {
            return 1;
        }

        /// <summary>
        /// Asynchronouses this instance.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Async()
        {
            return false;
        }

        /// <summary>
        /// Routings the key.
        /// </summary>
        /// <returns>System.String.</returns>
        public string RoutingKey()
        {
            return string.Empty;
        }
    }
}