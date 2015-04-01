// ***********************************************************************
// Assembly         : Uninf.CacheData
// Author           : cz
// Created          : 01-21-2015
//
// Last Modified By : cz
// Last Modified On : 02-27-2015
// ***********************************************************************
// <copyright file="CountBase.cs" company="Uninf">
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
    /// 数量缓存基类
    /// 在一对多关系中，通过主表主键，获取从表相关的数量
    /// </summary>
    /// <typeparam name="TMain">关系中的主表</typeparam>
    /// <typeparam name="TChild">关系中的从表</typeparam>
    /// <typeparam name="TMainKey">主表主键类型</typeparam>
    public abstract class CountBase<TMain, TChild, TMainKey> : ICountGetter<TMain, TChild, TMainKey>, ICountSetter<TMain, TChild, TMainKey>
    {
        /// <summary>
        /// 缓存操作
        /// </summary>
        private ICache cache;

        /// <summary>
        /// Initializes a new instance of the <see cref="CountBase{TMain, TChild, TMainKey}" /> class.
        /// </summary>
        /// <param name="cache">The cache.</param>
        protected CountBase(ICache cache)
        {
            this.cache = cache;
        }

        /// <summary>
        /// 获取数量
        /// </summary>
        /// <param name="key">主表主键</param>
        /// <returns>数量</returns>
        public virtual int GetCount(TMainKey key)
        {
            try
            {
                var cacheKey = CountCacheKey(key);
                if (cache.ContainsKey(cacheKey))
                {
                    return cache.Get<int>(cacheKey);
                }
                var cnt = RebuildCount(key);
                cache.Set(cacheKey, cnt);
                return cnt;
            }
            catch
            {
                return RebuildCount(key);
            }
        }

        /// <summary>
        /// 批量获取数量
        /// </summary>
        /// <param name="keys">主表主键集合</param>
        /// <returns>已字典形式返回数量，字典的key是主表主键，value是数量</returns>
        public virtual IDictionary<TMainKey, int> GetCounts(params TMainKey[] keys)
        {
            try
            {
                var cacheKeys = keys.Select(this.CountCacheKey);
                var dic=cache.GetAll<int?>(cacheKeys.ToArray());
                var empty = dic.Where(x => !x.Value.HasValue).Select(x => x.Key).Select(GetKeyFromCacheKey);
                if (empty.Any())
                {
                    var emptydic = RebuildCounts(empty.ToArray());
                    foreach (var item in emptydic)
                    {
                        dic[CountCacheKey(item.Key)] = item.Value;
                        cache.Set(CountCacheKey(item.Key), item.Value);
                    }
                }
                
                return dic.ToDictionary(x=>GetKeyFromCacheKey(x.Key),x=>x.Value.Value);
            }
            catch
            {
                return RebuildCounts(keys);
            }
        }

        /// <summary>
        /// 批量重建数量
        /// </summary>
        /// <param name="keys">主表主键</param>
        /// <returns>IDictionary&lt;TMainKey, System.Int32&gt;.</returns>
        protected abstract IDictionary<TMainKey, int> RebuildCounts(params TMainKey[] keys);

        /// <summary>
        /// 重建数量
        /// </summary>
        /// <param name="key">主表主键</param>
        /// <returns>System.Int32.</returns>
        protected abstract int RebuildCount(TMainKey key);

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public virtual void Increament(TMainKey key, int value = 1)
        {
            var cacheKey = CountCacheKey(key);
            if (cache.ContainsKey(cacheKey))
            {
                cache.Increment(cacheKey, (uint)value);
            }
        }

        /// <summary>
        /// 减少
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public virtual void Decreament(TMainKey key,int value = 1)
        {
            var cacheKey = CountCacheKey(key);
            if (cache.ContainsKey(cacheKey))
            {
                cache.Decrement(cacheKey, (uint)value);
            }
        }

        /// <summary>
        /// 缓存对应的key默认使用cnt:TMain/TChild/Name:key
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>System.String.</returns>
        protected virtual string CountCacheKey(TMainKey key)
        {
            return "cnt:" + typeof(TMain).Name + "/" + typeof(TChild).Name+"/"+CountName() + ":" + key;
        }

        /// <summary>
        /// 通过缓存key，解析主表主键，如主表主键无法从string转换，请在子类重写此方法
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>TMainKey.</returns>
        /// <exception cref="System.Exception">不支持string到 + typeof(TMainKey).Name + 的类型转换，请在+this.GetType().Name+中override GetKeyFromCacheKey方法</exception>
        protected virtual TMainKey GetKeyFromCacheKey(string key)
        {
            try
            {
                var arr = key.Split(':');
                var id = arr.Last();
                return (TMainKey)Convert.ChangeType(id, typeof(TMainKey));
            }
            catch
            {
                throw new Exception("不支持string到" + typeof(TMainKey).Name + "的类型转换，请在"+this.GetType().Name+"中override GetKeyFromCacheKey方法");
            }
        }

        /// <summary>
        /// 数量名称
        /// 同样的主从实体间可能有多种关系，如好友，关注都是人与人的关系，要为每种起不同的名字
        /// </summary>
        /// <returns>System.String.</returns>
        protected abstract string CountName();
    }
}