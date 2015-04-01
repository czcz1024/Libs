// ***********************************************************************
// Assembly         : Uninf.Auth
// Author           : cz
// Created          : 01-08-2015
//
// Last Modified By : cz
// Last Modified On : 01-08-2015
// ***********************************************************************
// <copyright file="IAuthInfo.cs" company="Uninf">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.Auth
{
    /// <summary>
    /// 标识为一个身份信息
    /// </summary>
    public interface IAuthInfo
    {
        /// <summary>
        /// 此种身份代表的角色
        /// </summary>
        /// <returns>System.String.</returns>
        string AuthRole();
    }
}