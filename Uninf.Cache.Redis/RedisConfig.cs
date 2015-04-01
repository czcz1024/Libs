// ***********************************************************************
// Assembly         : Uninf.Cache.Redis
// Author           : cz
// Created          : 01-12-2015
//
// Last Modified By : cz
// Last Modified On : 01-29-2015
// ***********************************************************************
// <copyright file="RedisConfig.cs" company="Uninf">
//     Copyright ©  2014
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.Cache.Redis
{
    using System;
    using System.Linq.Expressions;
    using System.Xml.Serialization;

    using ServiceStack.Redis;

    /// <summary>
    /// Class RedisConfig.
    /// </summary>
    public class RedisConfig:IRedisConfig
    {
        /// <summary>
        /// Gets or sets the connection strings.
        /// </summary>
        /// <value>The connection strings.</value>
        public string[] ConnectionStrings { get; set; }

        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <returns>System.String[].</returns>
        public virtual string[] GetConnection()
        {
            return ConnectionStrings;
        }

        /// <summary>
        /// Gets the prefix.
        /// </summary>
        /// <returns>System.String.</returns>
        public virtual string GetPrefix()
        {
            return KeyPrefix;
        }

        /// <summary>
        /// Gets the index of the database.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public virtual int GetDbIndex()
        {
            return DbIndex;
        }

        /// <summary>
        /// Gets or sets the connect time out.
        /// </summary>
        /// <value>The connect time out.</value>
        public int ConnectTimeOut { get; set; }

        /// <summary>
        /// Gets or sets the send time out.
        /// </summary>
        /// <value>The send time out.</value>
        public int SendTimeOut { get; set; }

        /// <summary>
        /// Gets or sets the recive time out.
        /// </summary>
        /// <value>The recive time out.</value>
        public int ReciveTimeOut { get; set; }

        /// <summary>
        /// Gets the connect time out.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public virtual int GetConnectTimeOut()
        {
            return ConnectTimeOut;
        }

        /// <summary>
        /// Gets the send time out.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public virtual int GetSendTimeOut()
        {
            return SendTimeOut;
        }

        /// <summary>
        /// Gets the recive time out.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public virtual int GetReciveTimeOut()
        {
            return ReciveTimeOut;
        }

        /// <summary>
        /// Gets the on read error.
        /// </summary>
        /// <returns>Action&lt;System.String, Exception&gt;.</returns>
        public virtual Action<string, Exception> GetOnReadError()
        {
            return OnReadError;;
        }

        /// <summary>
        /// Gets the on set error.
        /// </summary>
        /// <returns>Action&lt;System.String, Exception&gt;.</returns>
        public virtual Action<string, Exception> GetOnSetError()
        {
            return OnSetError;
        }

        /// <summary>
        /// Gets the client.
        /// </summary>
        /// <returns>IRedisClient.</returns>
        public virtual IRedisClient GetClient()
        {
            var clientsManager = new PooledRedisClientManager(this.GetDbIndex(), this.GetConnection())
            {
                NamespacePrefix = this.GetPrefix(),
                ConnectTimeout = this.GetConnectTimeOut(),
                SocketSendTimeout = this.GetSendTimeOut(),
                SocketReceiveTimeout = this.GetReciveTimeOut(),
            };
            return clientsManager.GetClient();
        }

        /// <summary>
        /// Gets or sets the key prefix.
        /// </summary>
        /// <value>The key prefix.</value>
        public string KeyPrefix { get; set; }

        /// <summary>
        /// Gets or sets the index of the database.
        /// </summary>
        /// <value>The index of the database.</value>
        public int DbIndex { get;set; }
        /// <summary>
        /// Gets or sets the on read error.
        /// </summary>
        /// <value>The on read error.</value>
        [XmlIgnore]
        public Action<string, Exception> OnReadError { get; set; }
        /// <summary>
        /// Gets or sets the on set error.
        /// </summary>
        /// <value>The on set error.</value>
        [XmlIgnore]
        public Action<string, Exception> OnSetError { get; set; }
    }
}