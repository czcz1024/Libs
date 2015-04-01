// ***********************************************************************
// Assembly         : Uninf.CacheData
// Author           : cz
// Created          : 01-07-2015
//
// Last Modified By : cz
// Last Modified On : 02-26-2015
// ***********************************************************************
// <copyright file="RelatedBase.cs" company="Uninf">
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
    /// RelatedBase. 类
    /// 主从关系
    /// </summary>
    /// <typeparam name="TMain">主实体</typeparam>
    /// <typeparam name="TChild">自实体类型，实体必须有名称为id的属性作为主键标识</typeparam>
    /// <typeparam name="TMainKey">主实体主键类型</typeparam>
    public abstract class RelatedBase<TMain,TChild,TMainKey>:IRelatedGetter<TChild,TMainKey>,IRelatedSetter<TChild,TMainKey>
    {
        /// <summary>
        /// The cache
        /// </summary>
        protected ICache cache;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelatedBase{TMain, TChild, TMainKey}"/> class.
        /// </summary>
        /// <param name="cache">The cache.</param>
        protected RelatedBase(ICache cache)
        {
            this.cache = cache;
        }

        /// <summary>
        /// 获取全部列表
        /// </summary>
        /// <param name="key">主实体主键</param>
        /// <returns>IEnumerable&lt;TChild&gt;.</returns>
        public virtual IEnumerable<TChild> GetRelated(TMainKey key)
        {
            try
            {
                var list = cache.GetAllRelated<TMain, TChild, TMainKey>(RelatedName(), key);
                if (list == null)
                {
                    list = Rebuild(key);
                    cache.SaveRelated<TMain, TChild, TMainKey>(RelatedName(), key, list.ToArray());
                }
                return list;
            }
            catch
            {
                return Rebuild(key);
            }
        }

        /// <summary>
        /// 重建全部列表
        /// </summary>
        /// <param name="key">主实体主键</param>
        /// <returns>IEnumerable&lt;TChild&gt;.</returns>
        protected abstract IEnumerable<TChild> Rebuild(TMainKey key);

        /// <summary>
        /// 分页获取列表
        /// </summary>
        /// <param name="key">主实体主键</param>
        /// <param name="skip">跳过数量</param>
        /// <param name="take">获取数量</param>
        /// <param name="all">全部数量</param>
        /// <param name="desc">if set to <c>true</c> [desc].</param>
        /// <returns>IEnumerable&lt;TChild&gt;.</returns>
        public virtual IEnumerable<TChild> GetRelatedPage(TMainKey key, int skip, int take, out long all, bool desc = true)
        {
            if (skip < 0) skip = 0;
            try
            {
                var list = cache.PageRelated<TMainKey, TChild, TMainKey>(RelatedName(), key, skip, take, out all, desc);
                var cacheCnt = 0;
                if(list!=null) cacheCnt=list.Count();
                if (list == null || !list.Any() || cacheCnt< take+skip)
                {
                    list = RebuildPage(key, skip, take, desc, out all);
                    if (skip > all)
                    {
                        skip = Convert.ToInt32((all / take)) * take;
                        list = RebuildPage(key, skip, take, desc, out all);
                    }
                    long tempall;
                    var rebuild = RebuildPage(key, cacheCnt, skip + take - cacheCnt, desc, out tempall);
                    if (rebuild.Any())
                    {
                        cache.SaveToPage<TMain, TChild, TMainKey>(RelatedName(), key, Score(), rebuild.ToArray());
                    }
                }
                else
                {
                    all = GetAllCount(key);
                }
                return list;
            }
            catch
            {
                var list= RebuildPage(key, skip, take, desc, out all);
                if (skip > all)
                {
                    skip = Convert.ToInt32((all / take)) * take;
                    list = RebuildPage(key, skip, take, desc, out all);
                }
                return list;
            }
        }

        /// <summary>
        /// 重建分页
        /// </summary>
        /// <param name="key">主实体主键</param>
        /// <param name="skip">跳过数量</param>
        /// <param name="take">获取数量</param>
        /// <param name="desc">if set to <c>true</c> [desc].</param>
        /// <param name="all">全部数量</param>
        /// <returns>IEnumerable&lt;TChild&gt;.</returns>
        protected abstract IEnumerable<TChild> RebuildPage(TMainKey key, int skip, int take, bool desc,out long all);

        /// <summary>
        /// 全部中添加
        /// </summary>
        /// <param name="pid">主实体主键</param>
        /// <param name="item">子实体</param>
        /// <param name="always">if set to <c>true</c> [always].</param>
        public void AddToRelated(TMainKey pid, TChild item, bool always = false)
        {
            if (always || cache.ContainsRelated<TMain, TChild>(RelatedName(), pid))
            {
                cache.AddToRelated<TMain, TChild, TMainKey>(RelatedName(), pid, item);
            }
        }

        /// <summary>
        /// 从全部中删除
        /// </summary>
        /// <param name="pid">主实体主键</param>
        /// <param name="item">子实体</param>
        public void RemoveFromRelated(TMainKey pid, TChild item)
        {
            cache.RemoveFromRelated<TMain, TChild, TMainKey>(RelatedName(), pid, item);
        }

        /// <summary>
        /// 分页中添加
        /// </summary>
        /// <param name="pid">主实体主键</param>
        /// <param name="item">子实体</param>
        /// <param name="always">当缓存不存在时，是否建立缓存，默认为否</param>
        public void AddToRelatedPage(TMainKey pid, TChild item, bool always = false)
        {
            bool hasDo = false;
            if (always ||cache.ContainsRelatedPage<TChild>(RelatedName(),pid))
            {
                cache.SaveToPage<TMain, TChild, TMainKey>(RelatedName(), pid, Score(), item);
                hasDo = true;
            }
            AfterAddToPage(pid, always, hasDo);
        }

        /// <summary>
        /// 添加到分页后
        /// </summary>
        /// <param name="pid">主实体主键</param>
        /// <param name="always">当缓存不存在时，是否建立缓存，默认为否</param>
        /// <param name="hasDo">是否真的添加到缓存了，如果always为true，则此参数一定为true</param>
        protected virtual void AfterAddToPage(TMainKey pid, bool always,bool hasDo)
        {
        }

        /// <summary>
        /// 从翻页删除
        /// </summary>
        /// <param name="pid">主实体主键</param>
        /// <param name="item">子实体</param>
        public void RemoveFromRelatedPage(TMainKey pid, TChild item)
        {
            var cnt=cache.RemoveFromPage<TMainKey, TChild, TMainKey>(this.RelatedName(), pid, item);
            AfterDeleteFromPage(pid, cnt);
        }

        /// <summary>
        /// 从翻页删除后
        /// </summary>
        /// <param name="pid">主实体主键</param>
        /// <param name="DeleteCnt">删除的个数</param>
        protected virtual void AfterDeleteFromPage(TMainKey pid, int DeleteCnt)
        {
        }

        /// <summary>
        /// 获取全部数量
        /// </summary>
        /// <param name="pid">主实体主键</param>
        /// <returns>System.Int64.</returns>
        protected abstract long GetAllCount(TMainKey pid);

        /// <summary>
        /// 关系名称
        /// </summary>
        /// <returns>System.String.</returns>
        protected abstract string RelatedName();

        /// <summary>
        /// 分页排序分数
        /// </summary>
        /// <returns>Func&lt;TChild, System.Int64&gt;.</returns>
        protected abstract Func<TChild, long> Score();
    }
}