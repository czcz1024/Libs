// ***********************************************************************
// Assembly         : Uninf.Auth
// Author           : cz
// Created          : 01-08-2015
//
// Last Modified By : cz
// Last Modified On : 01-08-2015
// ***********************************************************************
// <copyright file="IPrincipalSetter.cs" company="Uninf">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.Auth
{
    /// <summary>
    /// 设置身份信息到上下文的接口
    /// </summary>
    public interface IPrincipalSetter
    {
        /// <summary>
        /// 设置
        /// </summary>
        void SetToPrincipal();
    }
}