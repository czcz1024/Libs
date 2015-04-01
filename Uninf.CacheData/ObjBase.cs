// ***********************************************************************
// Assembly         : Uninf.CacheData
// Author           : cz
// Created          : 01-07-2015
//
// Last Modified By : cz
// Last Modified On : 01-15-2015
// ***********************************************************************
// <copyright file="ObjBase.cs" company="Uninf">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.CacheData
{
    using System.Collections.Generic;
    using System.Linq;

    using Uninf.Cache;

    /// <summary>
    /// ObjBase. 类
    /// </summary>
    /// <typeparam name="T">实体类型，实体必须有名称为id的属性作为主键标识</typeparam>
    /// <typeparam name="TKey">主键类型</typeparam>
    public abstract class ObjBase<T, TKey> : IObjGetter<T, TKey>, IObjSetter<T>
    {
        /// <summary>
        /// The cache
        /// </summary>
        protected ICache cache;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjBase{T, TKey}"/> class.
        /// </summary>
        /// <param name="cache">The cache.</param>
        protected ObjBase(ICache cache)
        {
            this.cache = cache;
        }

        /// <summary>
        /// 通过主键获取实体
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>T.</returns>
        public T Get(TKey key)
        {
            try
            {
                var obj = cache.Get<T, TKey>(key);
                if (obj == null)
                {
                    obj = Rebuild(key);
                    cache.Set(obj);
                }
                return obj;
            }
            catch
            {
                return Rebuild(key);
            }
        }

        /// <summary>
        /// 重建实体
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>T.</returns>
        protected abstract T Rebuild(TKey key);

        /// <summary>
        /// 批量获取实体
        /// </summary>
        /// <param name="keys">The keys.</param>
        /// <returns>IEnumerable&lt;T&gt;.</returns>
        public IEnumerable<T> Gets(params TKey[] keys)
        {
            try
            {
                var dic = cache.Gets<T, TKey>(keys);
                var empty = dic.Where(x => x.Value == null).Select(x => x.Key);
                var items = Rebuild(empty.ToArray());

                foreach (var item in items)
                {
                    dic[item.Key] = item.Value;
                    cache.Set(item.Value);
                }
                return dic.Select(x => x.Value).ToList();
            }
            catch
            {
                return Rebuild(keys).Select(x=>x.Value).ToList();
            }
        }

        /// <summary>
        /// 批量重建实体
        /// </summary>
        /// <param name="keys">The keys.</param>
        /// <returns>Dictionary&lt;TKey, T&gt;.</returns>
        protected abstract Dictionary<TKey,T> Rebuild(params TKey[] keys);

        /// <summary>
        /// 设置或更新
        /// </summary>
        /// <param name="item">The item.</param>
        public void Set(T item)
        {
            cache.Set(item);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="item">The item.</param>
        public void Delete(T item)
        {
            cache.Delete<T>(item);
        }
    }
}