// ***********************************************************************
// Assembly         : Uninf.CacheData
// Author           : cz
// Created          : 01-07-2015
//
// Last Modified By : cz
// Last Modified On : 01-21-2015
// ***********************************************************************
// <copyright file="IObjSetter.cs" company="Uninf">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.CacheData
{
    /// <summary>
    /// 缓存写入接口
    /// 用于批量依赖注入
    /// </summary>
    public interface ISetter
    {
    }

    /// <summary>
    /// 实体写入接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IObjSetter<T>:ISetter
    {
        /// <summary>
        /// 写入实体
        /// </summary>
        /// <param name="item">The item.</param>
        void Set(T item);

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="item">The item.</param>
        void Delete(T item);
        
    }

    /// <summary>
    /// 列表写入接口
    /// 全部列表和分页列表为不同的实现，向一个中添加不会出现在另一个中
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IListSetter<T> : ISetter
    {
        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="item">The item.</param>
        void AddToList(T item);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="item">The item.</param>
        void RemoveFromList(T item);

        /// <summary>
        /// 增加到分页
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="always">if set to <c>true</c> [always].</param>
        void AddToPage(T item, bool always = false);

        /// <summary>
        /// 从分页中删除
        /// </summary>
        /// <param name="item">The item.</param>
        void RemoveFromPage(T item);
    }

    /// <summary>
    /// 关系写入接口
    /// 全部列表和分页列表为不同的实现，向一个中添加不会出现在另一个中
    /// </summary>
    /// <typeparam name="TChild">The type of the t child.</typeparam>
    /// <typeparam name="TMainKey">The type of the t main key.</typeparam>
    public interface IRelatedSetter<TChild,TMainKey> : ISetter
    {
        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="pid">The pid.</param>
        /// <param name="item">The item.</param>
        /// <param name="always">if set to <c>true</c> [always].</param>
        void AddToRelated(TMainKey pid, TChild item,bool always=false);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="pid">The pid.</param>
        /// <param name="item">The item.</param>
        void RemoveFromRelated(TMainKey pid, TChild item);

        /// <summary>
        /// 增加到分页
        /// </summary>
        /// <param name="pid">The pid.</param>
        /// <param name="item">The item.</param>
        /// <param name="always">if set to <c>true</c> [always].</param>
        void AddToRelatedPage(TMainKey pid, TChild item, bool always = false);

        /// <summary>
        /// 从分页删除
        /// </summary>
        /// <param name="pid">The pid.</param>
        /// <param name="item">The item.</param>
        void RemoveFromRelatedPage(TMainKey pid, TChild item);
    }

    /// <summary>
    /// 数量写入接口
    /// </summary>
    /// <typeparam name="TMain">The type of the t main.</typeparam>
    /// <typeparam name="TChild">The type of the t child.</typeparam>
    /// <typeparam name="TMainKey">The type of the t main key.</typeparam>
    public interface ICountSetter<TMain, TChild, TMainKey> : ISetter
    {
        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        void Increament(TMainKey key,int value = 1);

        /// <summary>
        /// 减少
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        void Decreament(TMainKey key,int value = 1);
    }
}