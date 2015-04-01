// ***********************************************************************
// Assembly         : Uninf.Data.Lucene
// Author           : cz
// Created          : 01-21-2015
//
// Last Modified By : cz
// Last Modified On : 01-22-2015
// ***********************************************************************
// <copyright file="ILuceneSearch.cs" company="Uninf">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.Data.Lucene
{
    using System.Collections.Generic;

    /// <summary>
    /// ILuceneSearch 接口
    /// 为批量依赖注入注册提供
    /// </summary>
    public interface ILuceneSearch
    {
    }

    /// <summary>
    /// ILuceneSearch 接口
    /// </summary>
    /// <typeparam name="T">数据元素</typeparam>
    /// <typeparam name="TSearch">搜索方式</typeparam>
    public interface ILuceneSearch<T, in TSearch> : ILuceneSearch
    {
        /// <summary>
        /// 重建索引
        /// </summary>
        /// <param name="force">if set to <c>true</c> [force].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool Rebuild(bool force=false);

        /// <summary>
        /// 添加元素
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="force">if set to <c>true</c> [force].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool Add(T item, bool force = false);

        /// <summary>
        /// 更新元素
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="force">if set to <c>true</c> [force].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool Update(T item, bool force = false);

        /// <summary>
        /// 删除元素
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="force">if set to <c>true</c> [force].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool Delete(T item, bool force = false);

        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="search">The search.</param>
        /// <param name="skip">The skip.</param>
        /// <param name="take">The take.</param>
        /// <param name="all">All.</param>
        /// <returns>IEnumerable&lt;T&gt;.</returns>
        IEnumerable<T> Search(TSearch search,int skip,int take,out long all);
    }
}