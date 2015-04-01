// ***********************************************************************
// Assembly         : Uninf.Bus
// Author           : cz
// Created          : 01-20-2015
//
// Last Modified By : cz
// Last Modified On : 01-20-2015
// ***********************************************************************
// <copyright file="HandlerBase.cs" company="Uninf">
//     Copyright ©  2014
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.Bus
{
    /// <summary>
    /// HandlerBase. 类
    /// 无返回值消息处理器基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class HandlerBase<T>:IHandler<T>
    {
        /// <summary>
        /// The ser
        /// </summary>
        protected IMessageSerializer ser;

        /// <summary>
        /// Initializes a new instance of the <see cref="HandlerBase{T}"/> class.
        /// </summary>
        /// <param name="ser">The ser.</param>
        protected HandlerBase(IMessageSerializer ser)
        {
            this.ser = ser;
        }

        /// <summary>
        /// Handles the specified MSG.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        public abstract void Handle(T msg);

        /// <summary>
        /// Handles the specified MSG.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        public virtual void Handle(string msg)
        {
            if (SkipMsg(msg)) return;
            this.Handle(this.ser.Deserialize<T>(msg));
            SaveMsg(msg);
        }

        /// <summary>
        /// 是否跳过次消息
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <returns>跳过返回<c>true</c>，否则返回<c>false</c></returns>
        public virtual bool SkipMsg(string msg)
        {
            return false;
        }

        /// <summary>
        /// 保存消息，默认空实现。如果当处理成功后需要保存消息，可重写次方法
        /// </summary>
        /// <param name="msg">The MSG.</param>
        public virtual void SaveMsg(string msg)
        {
        }

        /// <summary>
        /// Sorts this instance.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public abstract int Sort();

        /// <summary>
        /// Asynchronouses this instance.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public abstract bool Async();

        /// <summary>
        /// Routings the key.
        /// </summary>
        /// <returns>System.String.</returns>
        public virtual string RoutingKey()
        {
            return typeof(T).FullName;
        }
    }
}