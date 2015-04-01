// ***********************************************************************
// Assembly         : Uninf.Cache.Redis
// Author           : cz
// Created          : 01-07-2015
//
// Last Modified By : cz
// Last Modified On : 01-26-2015
// ***********************************************************************
// <copyright file="RedisExtensions.cs" company="Uninf">
//     Copyright ©  2014
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.Cache.Redis
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using ServiceStack;
    using ServiceStack.Redis;
    using ServiceStack.Redis.Generic;
    using ServiceStack.Text;

    /// <summary>
    /// RedisExtensions. 类
    /// 对ServiceStack.Redis client的扩展方法
    /// 部分方法与类库中相同，因为类库中是internal所以移出来
    /// </summary>
    public static class RedisExtensions
    {
        /// <summary>
        /// Gets the type ids set key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redis">The redis.</param>
        /// <returns>System.String.</returns>
        public static string GetTypeIdsSetKey<T>(this IRedisClient redis)
        {
            var setkey = "";
            var client = redis as RedisClient;
            if (client != null)
            {
                setkey = client.GetTypeIdsSetKey<T>();
            }
            return setkey;
        }
        /// <summary>
        /// Gets the namespace prefix.
        /// </summary>
        /// <param name="redis">The redis.</param>
        /// <returns>System.String.</returns>
        public static string GetNamespacePrefix(this IRedisClient redis)
        {
            var namespacePrefix = "";
            var client = redis as RedisClient;
            if (client != null)
            {
                namespacePrefix = client.NamespacePrefix;
            }
            return namespacePrefix;
        }

        /// <summary>
        /// Gets the child reference set key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TChild">The type of the t child.</typeparam>
        /// <param name="redis">The redis.</param>
        /// <param name="name">The name.</param>
        /// <param name="parentId">The parent identifier.</param>
        /// <returns>System.String.</returns>
        public static string GetChildReferenceSetKey<T, TChild>(this IRedisClient redis, string name, object parentId)
        {

            return GetNamespacePrefix(redis) + (object)"ref:" + typeof(T).Name + "/" + typeof(TChild).Name + "/" + name + ":" + parentId;
        }

        /// <summary>
        /// Gets the child reference set key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TChild">The type of the t child.</typeparam>
        /// <param name="redis">The redis.</param>
        /// <param name="parentId">The parent identifier.</param>
        /// <returns>System.String.</returns>
        public static string GetChildReferenceSetKey<T, TChild>(this IRedisClient redis, object parentId)
        {
            return string.Concat(GetNamespacePrefix(redis), "ref:", typeof(T).Name, "/", typeof(TChild).Name, ":", parentId);
        }
        /// <summary>
        /// Urns the key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redis">The redis.</param>
        /// <param name="value">The value.</param>
        /// <returns>System.String.</returns>
        public static string UrnKey<T>(this IRedisClient redis, T value)
        {
            var namespacePrefix = GetNamespacePrefix(redis);
            return namespacePrefix + value.CreateUrn();
        }

        /// <summary>
        /// Gets the recent set key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="client">The client.</param>
        /// <param name="name">The name.</param>
        /// <param name="parentId">The parent identifier.</param>
        /// <returns>System.String.</returns>
        public static string GetRecentSetKey<T>(this IRedisClient client, string name, object parentId)
        {
            return string.Concat(GetNamespacePrefix(client), "recent:", typeof(T).Name, "/", name, ":", parentId);
        }

        /// <summary>
        /// Gets the recent set key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="client">The client.</param>
        /// <returns>System.String.</returns>
        public static string GetRecentSetKey<T>(this IRedisClient client)
        {
            return string.Concat(client.GetNamespacePrefix(), "recent:", typeof(T).Name);
        }

        /// <summary>
        /// Urns the key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redis">The redis.</param>
        /// <param name="id">The identifier.</param>
        /// <returns>System.String.</returns>
        private static string UrnKey<T>(IRedisClient redis, object id)
        {
            return String.Concat(GetNamespacePrefix(redis), IdUtils.CreateUrn<T>(id));
        }

        /// <summary>
        /// Gets the urn key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redis">The redis.</param>
        /// <param name="id">The identifier.</param>
        /// <returns>System.String.</returns>
        public static string GetUrnKey<T>(this IRedisClient redis, object id)
        {
            return UrnKey<T>(redis, id);
        }

        /// <summary>
        /// Stores the named related entities.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TChild">The type of the t child.</typeparam>
        /// <param name="redis">The redis.</param>
        /// <param name="name">The name.</param>
        /// <param name="parentId">The parent identifier.</param>
        /// <param name="children">The children.</param>
        public static void StoreNamedRelatedEntities<T, TChild>(this IRedisTypedClient<T> redis, string name, object parentId, List<TChild> children)
        {
            var childRefKey = GetChildReferenceSetKey<T, TChild>(redis.RedisClient, name, parentId);
            var childKeys = children.Select(x => UrnKey(redis.RedisClient, x)).ToList();

            using (var trans = redis.RedisClient.CreateTransaction())
            {
                //Ugly but need access to a generic constraint-free StoreAll method
                trans.QueueCommand(c => ((RedisClient)c).StoreAll(children));
                trans.QueueCommand(c => c.AddRangeToSet(childRefKey, childKeys));

                trans.Commit();
            }
        }
        /// <summary>
        /// Stores the named related entities.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TChild">The type of the t child.</typeparam>
        /// <param name="redis">The redis.</param>
        /// <param name="name">The name.</param>
        /// <param name="parentId">The parent identifier.</param>
        /// <param name="children">The children.</param>
        public static void StoreNamedRelatedEntities<T, TChild>(this IRedisTypedClient<T> redis, string name, object parentId, params TChild[] children)
        {
            redis.StoreNamedRelatedEntities(name, parentId, new List<TChild>(children));
        }

        /// <summary>
        /// Deletes the related entity fix.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TChild">The type of the t child.</typeparam>
        /// <param name="redis">The redis.</param>
        /// <param name="parentId">The parent identifier.</param>
        /// <param name="childId">The child identifier.</param>
        public static void DeleteRelatedEntityFix<T, TChild>(this IRedisTypedClient<T> redis, object parentId, object childId)
        {
            var childRefKey = GetChildReferenceSetKey<T, TChild>(redis.RedisClient, parentId);
            redis.RedisClient.RemoveItemFromSet(childRefKey, UrnKey<TChild>(redis.RedisClient, childId));
        }

        /// <summary>
        /// Deletes the named related entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TChild">The type of the t child.</typeparam>
        /// <param name="redis">The redis.</param>
        /// <param name="name">The name.</param>
        /// <param name="parentId">The parent identifier.</param>
        /// <param name="childId">The child identifier.</param>
        public static void DeleteNamedRelatedEntity<T, TChild>(this IRedisTypedClient<T> redis, string name, object parentId, object childId)
        {
            var childRefKey = GetChildReferenceSetKey<T, TChild>(redis.RedisClient, name, parentId);
            redis.RedisClient.RemoveItemFromSet(childRefKey, UrnKey<TChild>(redis.RedisClient, childId));
        }

        /// <summary>
        /// Deletes the named related entities.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TChild">The type of the t child.</typeparam>
        /// <param name="redis">The redis.</param>
        /// <param name="name">The name.</param>
        /// <param name="parentId">The parent identifier.</param>
        public static void DeleteNamedRelatedEntities<T, TChild>(this IRedisTypedClient<T> redis, string name, object parentId)
        {
            var childRefKey = GetChildReferenceSetKey<T, TChild>(redis.RedisClient, name, parentId);
            redis.RedisClient.Remove(childRefKey);
        }

        /// <summary>
        /// Gets the named related entities.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TChild">The type of the t child.</typeparam>
        /// <param name="redis">The redis.</param>
        /// <param name="name">The name.</param>
        /// <param name="parentId">The parent identifier.</param>
        /// <returns>List&lt;TChild&gt;.</returns>
        public static List<TChild> GetNamedRelatedEntities<T, TChild>(this IRedisTypedClient<T> redis, string name, object parentId)
        {
            var childRefKey = GetChildReferenceSetKey<T, TChild>(redis.RedisClient, name, parentId);
            var childKeys = redis.RedisClient.GetAllItemsFromSet(childRefKey).ToList();
            if (redis.ContainsKey(childRefKey))
            {
                return redis.RedisClient.As<TChild>().GetValues(childKeys);
            }
            return null;
        }

        /// <summary>
        /// Gets the named related entities count.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TChild">The type of the t child.</typeparam>
        /// <param name="redis">The redis.</param>
        /// <param name="name">The name.</param>
        /// <param name="parentId">The parent identifier.</param>
        /// <returns>System.Int64.</returns>
        public static long GetNamedRelatedEntitiesCount<T, TChild>(this IRedisTypedClient<T> redis, string name, object parentId)
        {
            var childRefKey = GetChildReferenceSetKey<T, TChild>(redis.RedisClient, name, parentId);
            return redis.RedisClient.GetSetCount(childRefKey);
        }

        /// <summary>
        /// Adds to recents list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redis">The redis.</param>
        /// <param name="name">The name.</param>
        /// <param name="parentId">The parent identifier.</param>
        /// <param name="value">The value.</param>
        public static void AddToRecentsList<T>(this IRedisTypedClient<T> redis, string name, object parentId, T value)
        {
            var key = UrnKey(redis.RedisClient, value);
            var nowScore = DateTime.UtcNow.ToUnixTime();
            var recentSetKey = GetRecentSetKey<T>(redis.RedisClient, name, parentId);
            redis.RedisClient.AddItemToSortedSet(recentSetKey, key, nowScore);
        }

        /// <summary>
        /// Gets the latest from recents list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redis">The redis.</param>
        /// <param name="name">The name.</param>
        /// <param name="parentId">The parent identifier.</param>
        /// <param name="skip">The skip.</param>
        /// <param name="take">The take.</param>
        /// <returns>List&lt;T&gt;.</returns>
        public static List<T> GetLatestFromRecentsList<T>(this IRedisTypedClient<T> redis, string name, object parentId, int skip, int take)
        {
            var toRank = take - 1;
            var recentSetKey = GetRecentSetKey<T>(redis.RedisClient, name, parentId);
            var keys = redis.RedisClient.GetRangeFromSortedSetDesc(recentSetKey, skip, toRank);
            var values = redis.GetValues(keys);
            return values;
        }

        /// <summary>
        /// Gets the earliest from recents list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redis">The redis.</param>
        /// <param name="name">The name.</param>
        /// <param name="parentId">The parent identifier.</param>
        /// <param name="skip">The skip.</param>
        /// <param name="take">The take.</param>
        /// <returns>List&lt;T&gt;.</returns>
        public static List<T> GetEarliestFromRecentsList<T>(this IRedisTypedClient<T> redis, string name, object parentId, int skip, int take)
        {
            var toRank = take - 1;
            var recentSetKey = GetRecentSetKey<T>(redis.RedisClient, name, parentId);
            var keys = redis.RedisClient.GetRangeFromSortedSet(recentSetKey, skip, toRank);
            var values = redis.GetValues(keys);
            return values;
        }
    }
}