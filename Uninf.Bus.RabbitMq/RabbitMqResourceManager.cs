// ***********************************************************************
// Assembly         : Uninf.Bus.RabbitMq
// Author           : cz
// Created          : 01-07-2015
//
// Last Modified By : cz
// Last Modified On : 01-07-2015
// ***********************************************************************
// <copyright file="RabbitMqResourceManager.cs" company="Uninf">
//     Copyright ©  2014
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.Bus.RabbitMq
{
    using System.Transactions;

    using RabbitMQ.Client;

    /// <summary>
    /// RabbitMqResourceManager. 类
    /// 提供rabbitmq的事务支持
    /// </summary>
    public class RabbitMqResourceManager : IEnlistmentNotification
    {
        /// <summary>
        /// The _channel
        /// </summary>
        private readonly IModel _channel;

        /// <summary>
        /// The _conn
        /// </summary>
        private IConnection _conn;
        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMqResourceManager"/> class.
        /// </summary>
        /// <param name="conn">The connection.</param>
        /// <param name="channel">The channel.</param>
        /// <param name="transaction">The transaction.</param>
        public RabbitMqResourceManager(IConnection conn,IModel channel, Transaction transaction)
        {
            _conn = conn;
            _channel = channel;
            _channel.TxSelect();
            transaction.EnlistVolatile(this, EnlistmentOptions.None);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMqResourceManager"/> class.
        /// </summary>
        /// <param name="conn">The connection.</param>
        /// <param name="channel">The channel.</param>
        public RabbitMqResourceManager(IConnection conn,IModel channel)
        {
            _conn = conn;
            _channel = channel;
            _channel.TxSelect();
            if (Transaction.Current != null)
                Transaction.Current.EnlistVolatile(this, EnlistmentOptions.None);
        }
        /// <summary>
        /// 通知登记的对象事务正在提交。
        /// </summary>
        /// <param name="enlistment">用于将响应发送到事务管理器的 <see cref="T:System.Transactions.Enlistment" /> 对象。</param>
        public void Commit(Enlistment enlistment)
        {
            _channel.TxCommit();
            enlistment.Done();
            _channel.Dispose();
            _conn.Dispose();
        }

        /// <summary>
        /// 通知登记的对象事务的状态不确定。
        /// </summary>
        /// <param name="enlistment">用于将响应发送到事务管理器的 <see cref="T:System.Transactions.Enlistment" /> 对象。</param>
        public void InDoubt(Enlistment enlistment)
        {
            Rollback(enlistment);
        }

        /// <summary>
        /// 通知登记的对象事务正在为提交做准备。
        /// </summary>
        /// <param name="preparingEnlistment">用于将响应发送到事务管理器的 <see cref="T:System.Transactions.PreparingEnlistment" /> 对象。</param>
        public void Prepare(PreparingEnlistment preparingEnlistment)
        {
            preparingEnlistment.Prepared();
        }

        /// <summary>
        /// 通知登记的对象事务正在回滚（中止）。
        /// </summary>
        /// <param name="enlistment">用于将响应发送到事务管理器的 <see cref="T:System.Transactions.Enlistment" /> 对象。</param>
        public void Rollback(Enlistment enlistment)
        {
            _channel.TxRollback();
            enlistment.Done();
            _channel.Dispose();
            _conn.Dispose();
        }
    }
}