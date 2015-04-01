// ***********************************************************************
// Assembly         : Uninf.Data
// Author           : cz
// Created          : 12-26-2014
//
// Last Modified By : cz
// Last Modified On : 01-08-2015
// ***********************************************************************
// <copyright file="IUnitOfWork.cs" company="Uninf">
//     Copyright ©  2014
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.Data
{
    using System;



    /// <summary>
    /// 工作单元接口
    /// </summary>
    public interface IUnitOfWork:IDisposable
    {
        /// <summary>
        /// 获取仓储
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <returns>IRepository&lt;T&gt;.</returns>
        IRepository<T> GetRepository<T>() where T:class;
        /// <summary>
        /// 保存修改
        /// </summary>
        /// <returns>影响的条数</returns>
        int SaveChanges();
    }
}