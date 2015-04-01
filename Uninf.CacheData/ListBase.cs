// ***********************************************************************
// Assembly         : Uninf.CacheData
// Author           : cz
// Created          : 01-07-2015
//
// Last Modified By : cz
// Last Modified On : 02-27-2015
// ***********************************************************************
// <copyright file="ListBase.cs" company="Uninf">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.CacheData
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Uninf.Cache;

    /// <summary>
    /// ListBase. 类
    /// </summary>
    /// <typeparam name="T">实体类型，实体必须有名称为id的属性作为主键标识</typeparam>
    public abstract class ListBase<T> : IListGetter<T>, IListSetter<T>
    {
        /// <summary>
        /// The cache
        /// </summary>
        protected ICache cache;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListBase{T}" /> class.
        /// </summary>
        /// <param name="cache">The cache.</param>
        public ListBase(ICache cache)
        {
            this.cache = cache;
        }

        /// <summary>
        /// 获取全部列表
        /// </summary>
        /// <returns>IEnumerable&lt;T&gt;.</returns>
        public virtual IEnumerable<T> GetList()
        {
            try
            {
                var list = cache.GetAll<T>();
                if (list == null)
                {
                    list = RebuildList();
                    cache.SaveToList(list);
                }
                return list;
            }
            catch
            {
                return RebuildList();
            }
        }

        /// <summary>
        /// 重建全部列表
        /// </summary>
        /// <returns>IEnumerable&lt;T&gt;.</returns>
        protected abstract IEnumerable<T> RebuildList();


        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="skip">跳过</param>
        /// <param name="take">获取条数</param>
        /// <param name="all">全部数量</param>
        /// <param name="desc">if set to <c>true</c> [desc].</param>
        /// <returns>IEnumerable&lt;T&gt;.</returns>
        public virtual IEnumerable<T> Page(int skip, int take, out long all, bool desc = true)
        {
            if (skip < 0) skip = 0;
            try
            {
                var list = cache.Page<T>(skip, take, out all, desc);
                var cachecnt = 0;
                if (list!=null)
                {
                    cachecnt=list.Count();
                }
                if (list == null || !list.Any() || cachecnt < (take+skip))
                {
                    list = RebuildPage(skip, take, desc, out all);
                    if (skip > all)
                    {
                        skip = Convert.ToInt32((all / take)) * take;
                        list = RebuildPage(skip, take, desc, out all);
                    }
                    long alltemp;
                    var rebuild = RebuildPage(cachecnt, skip + take - cachecnt, desc, out alltemp);
                    if (rebuild.Any())
                    {
                        cache.SaveToPage<T>(Score(), rebuild.ToArray());
                    }
                }
                else
                {
                    all = GetAllCount();
                }
                return list;
            }
            catch
            {

                var list= RebuildPage(skip, take, desc,out all);
                if (skip > all)
                {
                    skip = Convert.ToInt32((all / take)) * take;
                    list = RebuildPage(skip, take, desc, out all);
                }
                return list;
            }
        }

        /// <summary>
        /// 获取全部数量
        /// </summary>
        /// <returns>System.Int64.</returns>
        protected abstract long GetAllCount();


        /// <summary>
        /// 计算排序值
        /// </summary>
        /// <returns>Func&lt;T, System.Int64&gt;.</returns>
        protected abstract Func<T, long> Score();

        /// <summary>
        /// 重建分页
        /// </summary>
        /// <param name="skip">跳过</param>
        /// <param name="take">获取条数</param>
        /// <param name="desc">if set to <c>true</c> [desc].</param>
        /// <param name="all">全部数量</param>
        /// <returns>IEnumerable&lt;T&gt;.</returns>
        protected abstract IEnumerable<T> RebuildPage(int skip, int take, bool desc,out long all);

        /// <summary>
        /// 向全部列表中添加
        /// </summary>
        /// <param name="item">The item.</param>
        public void AddToList(T item)
        {
            cache.AddToList(item);
        }

        /// <summary>
        /// 从全部列表中删除
        /// </summary>
        /// <param name="item">The item.</param>
        public void RemoveFromList(T item)
        {
            cache.DeleteFromList(item);
        }

        /// <summary>
        /// 向分页中添加
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="always">if set to <c>true</c> [always].</param>
        public void AddToPage(T item,bool always=false)
        {
            var hasDo = false;
            if (always || cache.ContainsPage<T>())
            {
                cache.SaveToPage(Score(), item);
                hasDo = true;
            }
            AfterAddToPage(always, hasDo);
        }
        /// <summary>
        /// 添加到分页后
        /// </summary>
        /// <param name="always">if set to <c>true</c> [always].</param>
        /// <param name="hasDo">if set to <c>true</c> [has do].</param>
        protected virtual void AfterAddToPage(bool always, bool hasDo)
        {
        }

        /// <summary>
        /// 从分页中删除
        /// </summary>
        /// <param name="item">The item.</param>
        public void RemoveFromPage(T item)
        {
            var cnt=cache.RemoveFromPage(item);
            AfterDeleteFromPage(cnt);
        }

        /// <summary>
        /// 从分页删除后
        /// </summary>
        /// <param name="DeleteCnt">The delete count.</param>
        protected virtual void AfterDeleteFromPage(int DeleteCnt)
        {
        }
    }
}