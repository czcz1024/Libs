// ***********************************************************************
// Assembly         : Uninf.Auth
// Author           : cz
// Created          : 01-08-2015
//
// Last Modified By : cz
// Last Modified On : 01-08-2015
// ***********************************************************************
// <copyright file="IAuthOperator.cs" company="Uninf">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.Auth
{
    /// <summary>
    /// 身份验证流程需要用到的操作
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAuthOperator<T> where T : IAuthInfo
    {
        /// <summary>
        /// 加密器
        /// </summary>
        /// <returns>IAuthInfoEncryptor.</returns>
        IAuthInfoEncryptor Encryptor();

        /// <summary>
        /// 序列化器
        /// </summary>
        /// <returns>IAuthInfoSerializer&lt;T&gt;.</returns>
        IAuthInfoSerializer<T> Serializer();

        /// <summary>
        /// 存储器
        /// </summary>
        /// <returns>IAuthInfoStorage.</returns>
        IAuthInfoStorage Storager();

        /// <summary>
        /// 存储时，使用的key 或 name
        /// </summary>
        /// <returns>System.String.</returns>
        string StorageName();
    }
}