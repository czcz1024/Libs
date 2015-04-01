// ***********************************************************************
// Assembly         : Uninf.CacheData
// Author           : cz
// Created          : 01-13-2015
//
// Last Modified By : cz
// Last Modified On : 02-26-2015
// ***********************************************************************
// <copyright file="ManyRelatedBase.cs" company="Uninf">
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
    /// ManyRelatedBase. 类
    /// 父子孙3层关系
    /// </summary>
    /// <typeparam name="TChild">子类型</typeparam>
    /// <typeparam name="TMainKey">父主键类型</typeparam>
    /// <typeparam name="TManyKey">子主键类型</typeparam>
    /// <typeparam name="TOneGetter">子孙1对多关系缓存实现类，比如继承自IRelatedGetter&lt;TChild,TManyKey&gt;</typeparam>
    public abstract class ManyRelatedBase<TChild, TMainKey, TManyKey, TOneGetter> : IRelatedGetter< TChild, TMainKey>
        where TOneGetter:IRelatedGetter<TChild,TManyKey>
    {
        /// <summary>
        /// The cache
        /// </summary>
        protected ICache cache;

        /// <summary>
        /// The one getter
        /// </summary>
        protected TOneGetter oneGetter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManyRelatedBase{TChild, TMainKey, TManyKey, TOneGetter}"/> class.
        /// </summary>
        /// <param name="cache">The cache.</param>
        /// <param name="oneGetter">The one getter.</param>
        protected ManyRelatedBase(ICache cache, TOneGetter oneGetter)
        {
            this.cache = cache;
            this.oneGetter = oneGetter;
        }

        /// <summary>
        /// 通过父id获取孙列表
        /// </summary>
        /// <param name="key">父id</param>
        /// <returns>IEnumerable&lt;TChild&gt;.</returns>
        public virtual IEnumerable<TChild> GetRelated(TMainKey key)
        {
            var keys = GetManyKeys(key);
            var list = keys.SelectMany(x => oneGetter.GetRelated(x));
            return list;
        }

        /// <summary>
        /// 翻页获取
        /// </summary>
        /// <param name="key">父id</param>
        /// <param name="skip">跳过条数</param>
        /// <param name="take">获取条数</param>
        /// <param name="all">全部数量</param>
        /// <param name="desc">if set to <c>true</c> [desc].</param>
        /// <returns>IEnumerable&lt;TChild&gt;.</returns>
        public virtual IEnumerable<TChild> GetRelatedPage(TMainKey key,int skip,int take,out long all,bool desc=true)
        {
            if (skip < 0) skip = 0;
            var keys = GetManyKeys(key);
            var list= GetChildren(keys,skip,take,out all,desc);
            if (skip > all)
            {
                skip = Convert.ToInt32(all - take);
                list = GetChildren(keys, skip, take, out all, desc);
            }
            return list;
        }

        /// <summary>
        /// 通过子id集合获取孙列表
        /// </summary>
        /// <param name="keys">子id集合</param>
        /// <param name="skip">跳过条数</param>
        /// <param name="take">获取条数</param>
        /// <param name="all">全部数量</param>
        /// <param name="desc">if set to <c>true</c> [desc].</param>
        /// <returns>IEnumerable&lt;TChild&gt;.</returns>
        public virtual IEnumerable<TChild> GetChildren(
            IEnumerable<TManyKey> keys,
            int skip,
            int take,
            out long all,
            bool desc = true)
        {//need to fix
            
            var list = keys.SelectMany(
                x =>
                {
                    long itemCnt;
                    var list1 = oneGetter.GetRelatedPage(x, 0, skip+take, out itemCnt, desc);
                    
                    return list1;
                });
            all = GetAllCount(keys);
            return OrderBy(list).Skip(skip).Take(take);
        }

        /// <summary>
        /// 全部数量
        /// </summary>
        /// <param name="keys">The keys.</param>
        /// <returns>System.Int64.</returns>
        protected abstract long GetAllCount(IEnumerable<TManyKey> keys);

        /// <summary>
        ///排序
        /// </summary>
        /// <param name="src">The source.</param>
        /// <returns>IEnumerable&lt;TChild&gt;.</returns>
        protected abstract IEnumerable<TChild> OrderBy(IEnumerable<TChild> src);


        /// <summary>
        /// 通过父id获取子id集合
        /// </summary>
        /// <param name="mainKey">The main key.</param>
        /// <returns>IEnumerable&lt;TManyKey&gt;.</returns>
        protected abstract IEnumerable<TManyKey> GetManyKeys(TMainKey mainKey);

    }
}