// ***********************************************************************
// Assembly         : Uninf.CacheData
// Author           : cz
// Created          : 01-07-2015
//
// Last Modified By : cz
// Last Modified On : 01-21-2015
// ***********************************************************************
// <copyright file="IObjGetter.cs" company="Uninf">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.CacheData
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// 读缓存接口
    /// 用于批量注入
    /// </summary>
    public interface IGetter
    {
    }

    /// <summary>
    /// 实体类型读取接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TKey">The type of the t key.</typeparam>
    public interface IObjGetter<T,TKey>:IGetter
    {
        /// <summary>
        /// 读取实体
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>T.</returns>
        T Get(TKey key);

        /// <summary>
        /// 批量读取实体
        /// </summary>
        /// <param name="keys">The keys.</param>
        /// <returns>IEnumerable&lt;T&gt;.</returns>
        IEnumerable<T> Gets(params TKey[] keys);
    }

    /// <summary>
    /// 类表类型接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IListGetter<T> : IGetter
    {
        /// <summary>
        /// 读取列表
        /// </summary>
        /// <returns>IEnumerable&lt;T&gt;.</returns>
        IEnumerable<T> GetList();

        /// <summary>
        /// 翻页读取列表
        /// </summary>
        /// <param name="skip">The skip.</param>
        /// <param name="take">The take.</param>
        /// <param name="all">All.</param>
        /// <param name="desc">if set to <c>true</c> [desc].</param>
        /// <returns>IEnumerable&lt;T&gt;.</returns>
        IEnumerable<T> Page(int skip,int take,out long all,bool desc=true);
    }

    /// <summary>
    /// 一对多关系列表读取接口
    /// </summary>
    /// <typeparam name="TChild">The type of the t child.</typeparam>
    /// <typeparam name="TMainKey">The type of the t main key.</typeparam>
    public interface IRelatedGetter<TChild,TMainKey> : IGetter
    {
        /// <summary>
        /// 读取列表
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>IEnumerable&lt;TChild&gt;.</returns>
        IEnumerable<TChild> GetRelated(TMainKey key);
        /// <summary>
        /// 翻页读取列表
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="skip">The skip.</param>
        /// <param name="take">The take.</param>
        /// <param name="all">All.</param>
        /// <param name="desc">if set to <c>true</c> [desc].</param>
        /// <returns>IEnumerable&lt;TChild&gt;.</returns>
        IEnumerable<TChild> GetRelatedPage(TMainKey key,int skip,int take,out long all,bool desc=true);
    }

    /// <summary>
    /// 数量读取接口
    /// </summary>
    /// <typeparam name="TMain">The type of the t main.</typeparam>
    /// <typeparam name="TChild">The type of the t child.</typeparam>
    /// <typeparam name="TMainKey">The type of the t main key.</typeparam>
    public interface ICountGetter<TMain, TChild, TMainKey> : IGetter
    {
        /// <summary>
        /// 读取数量
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>System.Int32.</returns>
        int GetCount(TMainKey key);
    }
}