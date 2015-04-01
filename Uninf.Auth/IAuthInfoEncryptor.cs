// ***********************************************************************
// Assembly         : Uninf.Auth
// Author           : cz
// Created          : 01-08-2015
//
// Last Modified By : cz
// Last Modified On : 01-08-2015
// ***********************************************************************
// <copyright file="IAuthInfoEncryptor.cs" company="Uninf">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.Auth
{
    /// <summary>
    /// 身份信息的加密解密
    /// </summary>
    public interface IAuthInfoEncryptor
    {
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="src">The source.</param>
        /// <returns>System.String.</returns>
        string Encrypt(string src);
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="src">The source.</param>
        /// <returns>System.String.</returns>
        string Decrypt(string src);
    }
}