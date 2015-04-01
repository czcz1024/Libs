// ***********************************************************************
// Assembly         : Uninf.Cache.Redis
// Author           : cz
// Created          : 01-09-2015
//
// Last Modified By : cz
// Last Modified On : 02-27-2015
// ***********************************************************************
// <copyright file="RedisCache.cs" company="Uninf">
//     Copyright ©  2014
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.Cache.Redis
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using ServiceStack;
    using ServiceStack.Redis;

    /// <summary>
    /// RedisCache类
    /// </summary>
    public class RedisCache : ICache, IDisposable
    {
        /// <summary>
        /// The redis
        /// </summary>
        private IRedisClient redis;

        /// <summary>
        /// The configuration
        /// </summary>
        private IRedisConfig config;

        /// <summary>
        /// Initializes a new instance of the <see cref="RedisCache"/> class.
        /// </summary>
        /// <param name="config">The configuration.</param>
        public RedisCache(IRedisConfig config)
        {
            this.config = config;
            redis=config.GetClient();
        }

        //public RedisCache()
        //{
        //    var clientsManager = new PooledRedisClientManager(new string[]{"192.168.1.223"});
        //    this.redis = clientsManager.GetClient();
        //}
        //public RedisCache(string[] connectionString)
        //{
        //    var clientsManager = new PooledRedisClientManager(connectionString);
        //    this.redis = clientsManager.GetClient();
        //}
        /// <summary>
        /// 执行与释放或重置非托管资源相关的应用程序定义的任务。
        /// </summary>
        public void Dispose()
        {
            redis.Dispose();
        }

        #region 基本操作

        /// <summary>
        /// 获取缓存内容
        /// </summary>
        /// <param name="key">缓存key.</param>
        /// <returns>json</returns>
        /// <exception cref="Exception">如果缓存服务出错</exception>
        public string Get(string key)
        {
            try
            {
                return redis.GetEntry(key);
            }
            catch (Exception ex)
            {
                OnReadError(key, ex);
                throw;
            }
        }

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <returns>T.</returns>
        public T Get<T>(string key)
        {
            try
            {
                return redis.Get<T>(key);
            }
            catch (Exception ex)
            {
                OnReadError(key, ex);
                throw;
            }
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys">The keys.</param>
        /// <returns>IDictionary&lt;System.String, T&gt;.</returns>
        public IDictionary<string, T> GetAll<T>(params string[] keys)
        {
            return redis.GetAll<T>(keys);
        }

        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Set<T>(string key, T value)
        {
            try
            {
                redis.Set(key, value);
            }
            catch (Exception ex)
            {
                OnSetError(key, ex);
                throw;
            }
        }

        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="expire">The expire.</param>
        public void Set<T>(string key, T value, DateTime expire)
        {
            try
            {
                redis.Set(key, value, expire);
            }
            catch (Exception ex)
            {
                OnSetError(key, ex);
                throw;
            }
        }

        /// <summary>
        /// Increments the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>System.Int64.</returns>
        public long Increment(string key, uint value = 1)
        {
            try
            {
                return redis.Increment(key, value);
            }
            catch (Exception ex)
            {
                OnSetError(key, ex);
                throw;
            }
        }

        /// <summary>
        /// Decrements the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>System.Int64.</returns>
        public long Decrement(string key, uint value = 1)
        {
            try
            {
                return redis.Decrement(key, value);
            }
            catch (Exception ex)
            {
                OnSetError(key, ex);
                throw;
            }
        }

        /// <summary>
        /// Deletes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        public void Delete(string key)
        {
            try
            {
                redis.Remove(key);
            }
            catch (Exception ex)
            {
                OnSetError(key, ex);
                throw;
            }
        }

        /// <summary>
        /// Determines whether the specified key contains key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if the specified key contains key; otherwise, <c>false</c>.</returns>
        public bool ContainsKey(string key)
        {
            try
            {
                return redis.ContainsKey(key);
            }
            catch (Exception ex)
            {
                OnReadError(key, ex);
                throw;
            }
        }

        /// <summary>
        /// Expires at.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="datetime">The datetime.</param>
        public void ExpireAt(string key, DateTime datetime)
        {
            try
            {
                redis.ExpireEntryAt(key, datetime);
            }
            catch (Exception ex)
            {
                OnSetError(key, ex);
                throw;
            }
        }

        #endregion

        #region 实体操作
        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey">The type of the t key.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>T.</returns>
        public T Get<T, TKey>(TKey key)
        {
            try
            {
                return redis.As<T>().GetById(key);
            }
            catch (Exception ex)
            {
                var cacheKey = redis.GetUrnKey<T>(key);
                OnReadError(cacheKey, ex);
                throw;
            }
        }

        /// <summary>
        /// Getses the specified keys.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey">The type of the t key.</typeparam>
        /// <param name="keys">The keys.</param>
        /// <returns>Dictionary&lt;TKey, T&gt;.</returns>
        public Dictionary<TKey, T> Gets<T, TKey>(params TKey[] keys)
        {
            try
            {
                var item = redis.As<T>().GetByIds(keys);
                return keys.ToDictionary(x => x, x => item.FirstOrDefault(y => y.ToId().Equals(x)));
            }
            catch (Exception ex)
            {
                foreach (var k in keys)
                {
                    var ck = redis.GetUrnKey<T>(k);
                    OnReadError(ck, ex);

                }
                throw;
            }
        }

        /// <summary>
        /// Sets the specified item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The item.</param>
        public void Set<T>(T item)
        {
            var key = redis.UrnKey(item);
            try
            {
                redis.Set(key, item);
            }
            catch(Exception ex)
            {
                OnSetError(key, ex);
                throw;
            }
        }

        /// <summary>
        /// Sets the specified item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The item.</param>
        /// <param name="expire">The expire.</param>
        public void Set<T>(T item, DateTime expire)
        {
            var key = redis.UrnKey(item);
            try
            {
                redis.Set(key, item, expire);
            }
            catch(Exception ex)
            {
                OnSetError(key, ex);
                throw;
            }
        }

        /// <summary>
        /// Deletes the specified item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The item.</param>
        public void Delete<T>(T item)
        {
            try
            {
                redis.As<T>().Delete(item);
            }
            catch (Exception ex)
            {
                var key = redis.UrnKey(item);
                OnSetError(key, ex);
                throw;
            }
        }

        /// <summary>
        /// Deletes the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey">The type of the t key.</typeparam>
        /// <param name="key">The key.</param>
        public void Delete<T, TKey>(TKey key)
        {
            try
            {
                redis.As<T>().DeleteById(key);
            }
            catch (Exception ex)
            {
                var ck = redis.GetUrnKey<T>(key);
                OnSetError(ck, ex);
                throw;
            }
        }

        /// <summary>
        /// Deleteses the specified keys.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey">The type of the t key.</typeparam>
        /// <param name="keys">The keys.</param>
        public void Deletes<T, TKey>(params TKey[] keys)
        {
            try
            {
                redis.As<T>().DeleteByIds(keys);
            }
            catch (Exception ex)
            {
                foreach (var k in keys)
                {
                    var ck = redis.GetUrnKey<T>(k);
                    OnSetError(ck, ex);
                }
                throw;
            }
        }

        #endregion

        #region 列表操作

        /// <summary>
        /// Saves to list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The items.</param>
        public void SaveToList<T>(params T[] items)
        {
            try
            {
                redis.As<T>().StoreAll(items);
            }
            catch (Exception ex)
            {
                foreach (var item in items)
                {
                    var ck = redis.UrnKey(item);
                    OnSetError(ck, ex);
                }
                var idsk = redis.GetTypeIdsSetKey<T>();
                OnSetError(idsk, ex);
                throw;
            }
        }

        /// <summary>
        /// Adds to list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The item.</param>
        public void AddToList<T>(T item)
        {
            try
            {
                redis.As<T>().Store(item);
            }
            catch (Exception ex)
            {
                var key = redis.UrnKey(item);
                OnSetError(key, ex);
                var idsk = redis.GetTypeIdsSetKey<T>();
                OnSetError(idsk, ex);
                throw;
            }
        }

        /// <summary>
        /// Deletes from list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The item.</param>
        public void DeleteFromList<T>(T item)
        {
            try
            {
                redis.As<T>().Delete(item);
            }
            catch (Exception ex)
            {
                var key=redis.UrnKey(item);
                OnSetError(key, ex);
                var idsk = redis.GetTypeIdsSetKey<T>();
                OnSetError(idsk, ex);
                throw;
            }
        }

        /// <summary>
        /// Clears the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void ClearList<T>()
        {
            try
            {
                redis.As<T>().DeleteAll();
            }
            catch (Exception ex)
            {
                var k = redis.GetTypeIdsSetKey<T>();
                OnSetError(k, ex);
                throw;
            }
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>IEnumerable&lt;T&gt;.</returns>
        public IEnumerable<T> GetAll<T>()
        {
            if (!ContainsList<T>())
            {
                return null;
            }
            return redis.As<T>().GetAll();
        }

        /// <summary>
        /// Determines whether this instance contains list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns><c>true</c> if this instance contains list; otherwise, <c>false</c>.</returns>
        public bool ContainsList<T>()
        {
            var key = redis.GetTypeIdsSetKey<T>();
            if (key == "") return false;
            return redis.ContainsKey(key);
        }

        #endregion

        #region 列表操作非泛型

        /// <summary>
        /// Adds to list.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void AddToList(string key, string value)
        {
            try
            {
                redis.AddItemToList(key, value);
            }
            catch (Exception ex)
            {
                OnSetError(key, ex);
                throw;
            }
        }

        /// <summary>
        /// Removes from list.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void RemoveFromList(string key, string value)
        {
            try
            {
                redis.RemoveItemFromList(key, value);
            }
            catch (Exception ex)
            {
                OnSetError(key, ex);
                throw;
            }
        }

        /// <summary>
        /// Gets all from list.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>IEnumerable&lt;System.String&gt;.</returns>
        public IEnumerable<string> GetAllFromList(string key)
        {
            return redis.GetAllItemsFromList(key);
        }

        /// <summary>
        /// Gets the page from list.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="skip">The skip.</param>
        /// <param name="take">The take.</param>
        /// <returns>IEnumerable&lt;System.String&gt;.</returns>
        public IEnumerable<string> GetPageFromList(string key, int skip, int take)
        {
            return redis.GetRangeFromList(key, skip, skip + take);
        }

        /// <summary>
        /// Sets the item in list.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="index">The index.</param>
        /// <param name="value">The value.</param>
        public void SetItemInList(string key, int index, string value)
        {
            try
            {
                redis.SetItemInList(key, index, value);
            }
            catch (Exception ex)
            {
                OnSetError(key, ex);
                throw;
            }
        }

        /// <summary>
        /// Gets the item in list.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="index">The index.</param>
        /// <returns>System.String.</returns>
        public string GetItemInList(string key, int index)
        {
            return redis.GetItemFromList(key, index);
        }

        /// <summary>
        /// Clears the list.
        /// </summary>
        /// <param name="key">The key.</param>
        public void ClearList(string key)
        {
            try
            {
                redis.Remove(key);
            }
            catch(Exception ex)
            {
                OnSetError(key, ex);
                throw;
            }
        }

        #endregion

        #region 关系操作

        /// <summary>
        /// Saves the related.
        /// </summary>
        /// <typeparam name="TMain">The type of the t main.</typeparam>
        /// <typeparam name="TChild">The type of the t child.</typeparam>
        /// <typeparam name="TMainKey">The type of the t main key.</typeparam>
        /// <param name="name">The name.</param>
        /// <param name="key">The key.</param>
        /// <param name="items">The items.</param>
        public void SaveRelated<TMain, TChild, TMainKey>(string name, TMainKey key, params TChild[] items)
        {
            try
            {
                redis.As<TMain>().StoreNamedRelatedEntities<TMain, TChild>(name, key, items);
            }
            catch (Exception ex)
            {
                var ck = redis.GetChildReferenceSetKey<TMain, TChild>(name, key);
                OnSetError(ck, ex);
                throw;
            }
        }

        /// <summary>
        /// Adds to related.
        /// </summary>
        /// <typeparam name="TMain">The type of the t main.</typeparam>
        /// <typeparam name="TChild">The type of the t child.</typeparam>
        /// <typeparam name="TMainKey">The type of the t main key.</typeparam>
        /// <param name="name">The name.</param>
        /// <param name="key">The key.</param>
        /// <param name="item">The item.</param>
        public void AddToRelated<TMain, TChild, TMainKey>(string name, TMainKey key, TChild item)
        {
            try
            {
                SaveRelated<TMain, TChild, TMainKey>(name, key, item);
            }
            catch (Exception ex)
            {
                var ck = redis.GetChildReferenceSetKey<TMain, TChild>(name, key);
                OnSetError(ck, ex);
                throw;
            }
        }

        /// <summary>
        /// Removes from related.
        /// </summary>
        /// <typeparam name="TMain">The type of the t main.</typeparam>
        /// <typeparam name="TChild">The type of the t child.</typeparam>
        /// <typeparam name="TMainKey">The type of the t main key.</typeparam>
        /// <param name="name">The name.</param>
        /// <param name="key">The key.</param>
        /// <param name="item">The item.</param>
        public void RemoveFromRelated<TMain, TChild, TMainKey>(string name, TMainKey key, object item)
        {
            try
            {
                redis.As<TMain>().DeleteNamedRelatedEntity<TMain, TChild>(name, key, item);
            }
            catch (Exception ex)
            {
                var ck = redis.GetChildReferenceSetKey<TMain, TChild>(name, key);
                OnSetError(ck, ex);
                throw;
            }
        }

        /// <summary>
        /// Gets all related.
        /// </summary>
        /// <typeparam name="TMain">The type of the t main.</typeparam>
        /// <typeparam name="TChild">The type of the t child.</typeparam>
        /// <typeparam name="TMainKey">The type of the t main key.</typeparam>
        /// <param name="name">The name.</param>
        /// <param name="key">The key.</param>
        /// <returns>IEnumerable&lt;TChild&gt;.</returns>
        public IEnumerable<TChild> GetAllRelated<TMain, TChild, TMainKey>(string name, TMainKey key)
        {
            if (!ContainsRelated<TMain, TChild>(name, key))
            {
                return null;
            }
            return redis.As<TMain>().GetNamedRelatedEntities<TMain, TChild>(name, key);
        }

        /// <summary>
        /// Clears the related.
        /// </summary>
        /// <typeparam name="TMain">The type of the t main.</typeparam>
        /// <typeparam name="TChild">The type of the t child.</typeparam>
        /// <typeparam name="TMainKey">The type of the t main key.</typeparam>
        /// <param name="name">The name.</param>
        /// <param name="key">The key.</param>
        public void ClearRelated<TMain, TChild, TMainKey>(string name, TMainKey key)
        {
            var listkey = redis.GetChildReferenceSetKey<TMain, TChild>(name, key);
            var setKey = redis.GetRecentSetKey<TChild>(name, key);
            try
            {
                redis.Remove(listkey);
                redis.Remove(setKey);
            }
            catch (Exception ex)
            {
                OnSetError(listkey, ex);
                OnSetError(setKey, ex);
                throw;
            }
        }

        /// <summary>
        /// Determines whether the specified name contains related.
        /// </summary>
        /// <typeparam name="TMain">The type of the t main.</typeparam>
        /// <typeparam name="TChild">The type of the t child.</typeparam>
        /// <param name="name">The name.</param>
        /// <param name="pid">The pid.</param>
        /// <returns><c>true</c> if the specified name contains related; otherwise, <c>false</c>.</returns>
        public bool ContainsRelated<TMain, TChild>(string name, object pid)
        {
            var key = redis.GetChildReferenceSetKey<TMain, TChild>(name, pid);
            return redis.ContainsKey(key);
        }

        #endregion

        #region 分页

        /// <summary>
        /// Saves to page.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="score">The score.</param>
        /// <param name="items">The items.</param>
        public void SaveToPage<T>(Func<T, long> score, params T[] items)
        {
            var setKey = redis.GetRecentSetKey<T>();
            //var set = redis.As<T>().SortedSets[setKey];
            try
            {
                foreach (var item in items)
                {
                    var itemkey = redis.UrnKey(item);
                    //redis.As<T>().AddItemToSortedSet(set, item,score(item));
                    if (!redis.ContainsKey(itemkey))
                    {
                        redis.As<T>().Store(item);
                    }
                    redis.AddItemToSortedSet(setKey, itemkey, score(item));

                }
            }
            catch (Exception ex)
            {
                OnSetError(setKey, ex);
                throw;
            }
        }

        /// <summary>
        /// Saves to page.
        /// </summary>
        /// <typeparam name="TMain">The type of the t main.</typeparam>
        /// <typeparam name="TChild">The type of the t child.</typeparam>
        /// <typeparam name="TMainKey">The type of the t main key.</typeparam>
        /// <param name="name">The name.</param>
        /// <param name="key">The key.</param>
        /// <param name="score">The score.</param>
        /// <param name="items">The items.</param>
        public void SaveToPage<TMain, TChild, TMainKey>(string name, TMainKey key, Func<TChild, long> score, params TChild[] items)
        {
            var setKey = redis.GetRecentSetKey<TChild>(name, key);
            //var set = redis.As<TChild>().SortedSets[setKey];
            try
            {
                foreach (var item in items)
                {
                    var itemkey = redis.UrnKey(item);
                    if (!redis.ContainsKey(itemkey))
                    {
                        redis.As<TChild>().Store(item);
                    }
                    //redis.As<TChild>().AddItemToSortedSet(set, item, score(item));
                    redis.AddItemToSortedSet(setKey, itemkey, score(item));
                }
            }
            catch (Exception ex)
            {
                OnSetError(setKey, ex);
                throw;
            }
        }

        /// <summary>
        /// Removes from page.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The items.</param>
        /// <returns>System.Int32.</returns>
        public int RemoveFromPage<T>(params T[] items)
        {
            var setKey = redis.GetRecentSetKey<T>();
            //var set = redis.As<T>().SortedSets[setKey];
            try
            {
                return
                    items.Select(item => redis.UrnKey(item))
                        .Count(itemkey => redis.RemoveItemFromSortedSet(setKey, itemkey));
            }
            catch (Exception ex)
            {
                OnSetError(setKey, ex);
                throw;
            }
        }

        /// <summary>
        /// Removes from page.
        /// </summary>
        /// <typeparam name="TMain">The type of the t main.</typeparam>
        /// <typeparam name="TChild">The type of the t child.</typeparam>
        /// <typeparam name="TMainKey">The type of the t main key.</typeparam>
        /// <param name="name">The name.</param>
        /// <param name="key">The key.</param>
        /// <param name="items">The items.</param>
        /// <returns>System.Int32.</returns>
        public int RemoveFromPage<TMain, TChild, TMainKey>(string name, TMainKey key, params TChild[] items)
        {
            var setKey = redis.GetRecentSetKey<TChild>(name, key);
            //var set = redis.As<TChild>().SortedSets[setKey];
            try
            {
                return
                    items.Select(item => redis.UrnKey(item))
                        .Count(itemkey => redis.RemoveItemFromSortedSet(setKey, itemkey));
            }
            catch (Exception ex)
            {
                OnSetError(setKey, ex);
                throw;
            }
        }

        /// <summary>
        /// Clears the page.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void ClearPage<T>()
        {
            var setKey = redis.GetRecentSetKey<T>();
            try
            {
                redis.Remove(setKey);
            }
            catch (Exception ex)
            {
                OnSetError(setKey, ex);
                throw;
            }
        }

        /// <summary>
        /// Clears the page.
        /// </summary>
        /// <typeparam name="TMain">The type of the t main.</typeparam>
        /// <typeparam name="TChild">The type of the t child.</typeparam>
        /// <typeparam name="TMainKey">The type of the t main key.</typeparam>
        /// <param name="name">The name.</param>
        /// <param name="key">The key.</param>
        public void ClearPage<TMain, TChild, TMainKey>(string name, TMainKey key)
        {
            var setKey = redis.GetRecentSetKey<TChild>(name, key);
            try
            {
                redis.Remove(setKey);
            }
            catch (Exception ex)
            {
                OnSetError(setKey, ex);
                throw;
            }
        }


        /// <summary>
        /// Pages the specified skip.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="skip">The skip.</param>
        /// <param name="take">The take.</param>
        /// <param name="allCount">All count.</param>
        /// <param name="desc">if set to <c>true</c> [desc].</param>
        /// <returns>IEnumerable&lt;T&gt;.</returns>
        public IEnumerable<T> Page<T>(int skip, int take, out long allCount, bool desc)
        {
            if (!ContainsPage<T>())
            {
                allCount = 0;
                return null;
            }
            var setKey = redis.GetRecentSetKey<T>();
            allCount = redis.GetSortedSetCount(setKey);
            if (desc)
            {
                return redis.As<T>().GetLatestFromRecentsList(skip, take);
            }
            else
            {
                return redis.As<T>().GetEarliestFromRecentsList(skip, take);
            }
        }

        /// <summary>
        /// Pages the related.
        /// </summary>
        /// <typeparam name="TMain">The type of the t main.</typeparam>
        /// <typeparam name="TChild">The type of the t child.</typeparam>
        /// <typeparam name="TMainKey">The type of the t main key.</typeparam>
        /// <param name="name">The name.</param>
        /// <param name="pid">The pid.</param>
        /// <param name="skip">The skip.</param>
        /// <param name="take">The take.</param>
        /// <param name="allCount">All count.</param>
        /// <param name="desc">if set to <c>true</c> [desc].</param>
        /// <returns>IEnumerable&lt;TChild&gt;.</returns>
        public IEnumerable<TChild> PageRelated<TMain, TChild, TMainKey>(
            string name,
            object pid,
            int skip,
            int take,
            out long allCount,
            bool desc)
        {
            if (!ContainsRelatedPage<TChild>(name, pid))
            {
                allCount = 0;
                return null;
            }
            var setkey = redis.GetRecentSetKey<TChild>(name, pid);
            allCount = redis.GetSortedSetCount(setkey);
            if (desc)
            {
                return redis.As<TChild>().GetLatestFromRecentsList<TChild>(name, pid, skip, take);
            }
            else
            {
                return redis.As<TChild>().GetEarliestFromRecentsList<TChild>(name, pid, skip, take);
            }
        }

        /// <summary>
        /// Determines whether this instance contains page.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns><c>true</c> if this instance contains page; otherwise, <c>false</c>.</returns>
        public bool ContainsPage<T>()
        {
            var key = redis.GetRecentSetKey<T>();
            try
            {
                return redis.ContainsKey(key);
            }
            catch( Exception ex)
            {
                OnSetError(key, ex);
                throw;
            }
        }

        /// <summary>
        /// Determines whether [contains related page] [the specified name].
        /// </summary>
        /// <typeparam name="TChild">The type of the t child.</typeparam>
        /// <param name="name">The name.</param>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if [contains related page] [the specified name]; otherwise, <c>false</c>.</returns>
        public bool ContainsRelatedPage<TChild>(string name, object key)
        {
            var setkey = redis.GetRecentSetKey<TChild>(name, key);
            try
            {
                return redis.ContainsKey(setkey);
            }
            catch (Exception ex)
            {
                OnSetError(setkey, ex);
                throw;
            }
        }

        #endregion

        #region on error

        /// <summary>
        /// 写操作报错时执行
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="ex">The ex.</param>
        public virtual void OnSetError(string key, Exception ex)
        {
            var errOP = config.GetOnSetError();
            if (errOP != null)
            {
                var st = new StackTrace();
                var method = st.GetFrame(1).GetMethod().Name;
                var err = new Exception(ex.Message + ";" + method,ex);
                errOP(key, err);
            }
        }

        /// <summary>
        /// 读操作保存时执行
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="ex">The ex.</param>
        public virtual void OnReadError(string key, Exception ex)
        {
            var errOP = config.GetOnReadError();
            if (errOP != null)
            {
                var st = new StackTrace();
                var method = st.GetFrame(1).GetMethod().Name;
                var err = new Exception(ex.Message + ";" + method, ex);
                errOP(key, err);
            }
        }

        #endregion
    }
}