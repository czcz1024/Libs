// ***********************************************************************
// Assembly         : Uninf.Auth
// Author           : cz
// Created          : 01-08-2015
//
// Last Modified By : cz
// Last Modified On : 01-08-2015
// ***********************************************************************
// <copyright file="IAuthInfoSerializer.cs" company="Uninf">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.Auth
{
    /// <summary>
    /// 身份信息的序列化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAuthInfoSerializer<T> where T : IAuthInfo
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>System.String.</returns>
        string Serialize(T obj);
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="src">The source.</param>
        /// <returns>T.</returns>
        T Deserialize(string src);
    }
}