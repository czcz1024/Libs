// ***********************************************************************
// Assembly         : Uninf.Auth
// Author           : cz
// Created          : 01-08-2015
//
// Last Modified By : cz
// Last Modified On : 01-08-2015
// ***********************************************************************
// <copyright file="IAuthInfoStorage.cs" company="Uninf">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.Auth
{
    /// <summary>
    /// 身份信息在客户端的存储
    /// </summary>
    public interface IAuthInfoStorage
    {
        /// <summary>
        /// 保存到客户端
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        void Save(string name, string value);
        /// <summary>
        /// 从客户端读取
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>System.String.</returns>
        string Load(string name);
        /// <summary>
        /// 清除客户端保存的数据
        /// </summary>
        /// <param name="name">The name.</param>
        void Clear(string name);
    }
}