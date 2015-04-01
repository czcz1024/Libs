// ***********************************************************************
// Assembly         : Uninf.Data
// Author           : cz
// Created          : 12-26-2014
//
// Last Modified By : cz
// Last Modified On : 01-08-2015
// ***********************************************************************
// <copyright file="IRepository.cs" company="Uninf">
//     Copyright ©  2014
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.Data
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;



    /// <summary>
    /// 仓储接口
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public interface IRepository<T>
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>T.</returns>
        T Add(T entity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>T.</returns>
        T Delete(T entity);

        /// <summary>
        /// 通过主键查询
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>T.</returns>
        T Find(params object[] key);

        /// <summary>
        /// 转换为IQueryable接口，使得可用Linq进行延迟查询
        /// </summary>
        /// <returns>IQueryable&lt;T&gt;.</returns>
        IQueryable<T> AsQueryable();

        /// <summary>
        /// 整体更新
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>T.</returns>
        T Update(T entity);

        /// <summary>
        /// 限定字段更新
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="updateProperties">The update properties.</param>
        /// <returns>T.</returns>
        T Update(T entity, params Expression<Func<T, object>>[] updateProperties);
    }
}