// ***********************************************************************
// Assembly         : Uninf.Cache.Redis
// Author           : cz
// Created          : 01-12-2015
//
// Last Modified By : cz
// Last Modified On : 01-29-2015
// ***********************************************************************
// <copyright file="IRedisConfig.cs" company="Uninf">
//     Copyright ©  2014
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.Cache.Redis
{
    using System;

    using ServiceStack.Redis;

    /// <summary>
    /// redis配置
    /// </summary>
    public interface IRedisConfig
    {
        /// <summary>
        /// 获取连接
        /// </summary>
        /// <returns>System.String[].</returns>
        string[] GetConnection();

        /// <summary>
        /// 获取key前缀
        /// </summary>
        /// <returns>System.String.</returns>
        string GetPrefix();

        /// <summary>
        /// 获取db index
        /// </summary>
        /// <returns>System.Int32.</returns>
        int GetDbIndex();

        /// <summary>
        /// 获取连接超时时间
        /// </summary>
        /// <returns>System.Int32.</returns>
        int GetConnectTimeOut();

        /// <summary>
        /// 获取发送超时时间
        /// </summary>
        /// <returns>System.Int32.</returns>
        int GetSendTimeOut();

        /// <summary>
        /// 获取读取超时时间
        /// </summary>
        /// <returns>System.Int32.</returns>
        int GetReciveTimeOut();

        /// <summary>
        /// 读操作错误时执行
        /// </summary>
        /// <returns>Action&lt;System.String, Exception&gt;.</returns>
        Action<string, Exception> GetOnReadError();

        /// <summary>
        /// 写操作错误时执行
        /// </summary>
        /// <returns>Action&lt;System.String, Exception&gt;.</returns>
        Action<string, Exception> GetOnSetError();

        /// <summary>
        /// 获取redis client
        /// </summary>
        /// <returns>IRedisClient.</returns>
        IRedisClient GetClient();
    }
}