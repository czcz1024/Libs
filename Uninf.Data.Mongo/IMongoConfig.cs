// ***********************************************************************
// Assembly         : Uninf.Data.Mongo
// Author           : cz
// Created          : 01-20-2015
//
// Last Modified By : cz
// Last Modified On : 01-20-2015
// ***********************************************************************
// <copyright file="IMongoConfig.cs" company="Uninf">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.Data.Mongo
{
    /// <summary>
    /// IMongoConfig 接口
    /// </summary>
    public interface IMongoConfig
    {
        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <returns>System.String.</returns>
        string GetConnectionString();

        /// <summary>
        /// Gets the database.
        /// </summary>
        /// <returns>System.String.</returns>
        string GetDatabase();
    }

    /// <summary>
    /// MongoConfig. 类
    /// </summary>
    public class MongoConfig:IMongoConfig
    {
        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <returns>System.String.</returns>
        public string GetConnectionString()
        {
            return ConnectionString;
        }

        /// <summary>
        /// Gets the database.
        /// </summary>
        /// <returns>System.String.</returns>
        public string GetDatabase()
        {
            return Database;
        }

        /// <summary>
        /// Gets or sets the database.
        /// </summary>
        /// <value>The database.</value>
        public string Database { get; set; }

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        public string ConnectionString { get; set; }
    }
}