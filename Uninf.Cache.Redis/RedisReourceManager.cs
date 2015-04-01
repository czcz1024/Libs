// ***********************************************************************
// Assembly         : Uninf.Cache.Redis
// Author           : cz
// Created          : 01-23-2015
//
// Last Modified By : cz
// Last Modified On : 02-27-2015
// ***********************************************************************
// <copyright file="RedisReourceManager.cs" company="Uninf">
//     Copyright ©  2014
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.Cache.Redis
{
    using System.Transactions;

    using ServiceStack.Redis;

    /// <summary>
    /// RedisReourceManager. 类
    /// 为在TransctionScope中支持redis写操作事务提供的类
    /// </summary>
    public class RedisReourceManager : IEnlistmentNotification
    {
        /// <summary>
        /// The trans
        /// </summary>
        private IRedisTransaction trans;

        /// <summary>
        /// Initializes a new instance of the <see cref="RedisReourceManager" /> class.
        /// </summary>
        /// <param name="trans">The trans.</param>
        /// <param name="transaction">The transaction.</param>
        public RedisReourceManager(IRedisTransaction trans, Transaction transaction)
        {
            this.trans = trans;
            transaction.EnlistVolatile(this, EnlistmentOptions.None);
        }

        /// <summary>
        /// 通知登记的对象事务正在提交。
        /// </summary>
        /// <param name="enlistment">用于将响应发送到事务管理器的 <see cref="T:System.Transactions.Enlistment" /> 对象。</param>
        public virtual void Commit(Enlistment enlistment)
        {
            trans.Commit();
            enlistment.Done();
            trans.Dispose();
        }

        /// <summary>
        /// 通知登记的对象事务的状态不确定。
        /// </summary>
        /// <param name="enlistment">用于将响应发送到事务管理器的 <see cref="T:System.Transactions.Enlistment" /> 对象。</param>
        public virtual void InDoubt(Enlistment enlistment)
        {
            Rollback(enlistment);
        }

        /// <summary>
        /// 通知登记的对象事务正在为提交做准备。
        /// </summary>
        /// <param name="preparingEnlistment">用于将响应发送到事务管理器的 <see cref="T:System.Transactions.PreparingEnlistment" /> 对象。</param>
        public virtual void Prepare(PreparingEnlistment preparingEnlistment)
        {
            preparingEnlistment.Prepared();
        }

        /// <summary>
        /// 通知登记的对象事务正在回滚（中止）。
        /// </summary>
        /// <param name="enlistment">用于将响应发送到事务管理器的 <see cref="T:System.Transactions.Enlistment" /> 对象。</param>
        public virtual void Rollback(Enlistment enlistment)
        {
            trans.Rollback();
            enlistment.Done();
            trans.Dispose();
        }
    }
}