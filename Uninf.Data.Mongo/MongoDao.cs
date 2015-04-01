// ***********************************************************************
// Assembly         : Uninf.Data.Mongo
// Author           : cz
// Created          : 01-20-2015
//
// Last Modified By : cz
// Last Modified On : 01-20-2015
// ***********************************************************************
// <copyright file="MongoDao.cs" company="Uninf">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.Data.Mongo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using MongoDB.Bson;
    using MongoDB.Driver;
    using MongoDB.Driver.Builders;
    using MongoDB.Driver.Linq;

    /// <summary>
    /// MongoDao. 类
    /// </summary>
    public class MongoDao
    {
        /// <summary>
        /// The configuration
        /// </summary>
        private IMongoConfig config;

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoDao"/> class.
        /// </summary>
        /// <param name="config">The configuration.</param>
        public MongoDao(IMongoConfig config)
        {
            this.config = config;
        }

        /// <summary>
        /// 获取MongoDatabase对象
        /// </summary>
        /// <returns>MongoDatabase.</returns>
        public MongoDatabase GetDataBase()
        {
            var connectionString = config.GetConnectionString();
            var client = new MongoClient(connectionString);
            var server = client.GetServer();
            var database = server.GetDatabase(config.GetDatabase());
            return database;
        }

        /// <summary>
        /// 获取MongoCollection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>MongoCollection&lt;T&gt;.</returns>
        public MongoCollection<T> GetCollection<T>() where T : MongoEntityBase
        {
            var db = GetDataBase();
            
            return db.GetCollection<T>(typeof (T).Name);
        }
        /// <summary>
        /// Ensures the index.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <param name="isUnique">if set to <c>true</c> [is unique].</param>
        public void EnsureIndex<T>(Expression<Func<T, object>> predicate, bool isUnique) where T : MongoEntityBase
        {
            try
            {
                var collection = GetCollection<T>();
                var keysBuilder = new IndexKeysBuilder<T>();
                var keys = keysBuilder.Ascending(predicate);
                if (isUnique)
                {
                    var option = new IndexOptionsBuilder();
                    option.SetUnique(true);
                    collection.EnsureIndex(keys, option);
                }
                else
                {
                    collection.EnsureIndex(keys);
                }
            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// 以IQueryable形式获取Collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>IQueryable&lt;T&gt;.</returns>
        public IQueryable<T> GetQueryableCollection<T>() where T : MongoEntityBase
        {
            return GetCollection<T>().AsQueryable();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <returns>T.</returns>
        public T Add<T>(T obj) where T : MongoEntityBase
        {
            var collection = GetCollection<T>();
            collection.Insert(obj);
            return obj;
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objs">The objs.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool BatchAdd<T>(IList<T> objs) where T : MongoEntityBase
        {
            try
            {
                var collection = GetCollection<T>();
                collection.InsertBatch(objs);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <returns>T.</returns>
        public T Update<T>(T obj) where T : MongoEntityBase
        {
            var collection = GetCollection<T>();
            collection.Save(obj);
            return obj;
        }

        /// <summary>
        /// 条件更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <param name="update">The update.</param>
        public void Update<T>(Expression<Func<T, bool>> predicate,IEnumerable<KeyValuePair< Expression<Func<T,object>>,object>> update) where T : MongoEntityBase
        {
            var qb = new QueryBuilder<T>();
            var query = qb.Where(predicate);
            var collection = GetCollection<T>();
            //var update=MongoDB.Driver.Builders.Update<T>
            var up = new UpdateBuilder<T>();
            foreach (var item in update)
            {
                up = up.Set(item.Key, item.Value);
            }
            collection.Update(query,up);
        }

        /// <summary>
        /// 条件更新某一字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <param name="update">The update.</param>
        /// <param name="value">The value.</param>
        public void UpdateIncAll<T>(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> update, int value) where T : MongoEntityBase
        {
            var qb = new QueryBuilder<T>();
            var query = qb.Where(predicate);
            var collection = GetCollection<T>();
            //var update=MongoDB.Driver.Builders.Update<T>
            var up = new UpdateBuilder<T>();
            up = up.Inc(update, value);
            collection.Update(query, up, UpdateFlags.Multi | UpdateFlags.Upsert);
        }

        /// <summary>
        /// 主键获取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">The identifier.</param>
        /// <returns>T.</returns>
        public T GetByKey<T>(ObjectId id) where T : MongoEntityBase
        {
            var query = Query<T>.EQ(e => e.Id, id);
            var collection = GetCollection<T>();
            var obj = collection.FindOne(query);
            return obj;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <returns>IEnumerable&lt;T&gt;.</returns>
        public IEnumerable<T> Search<T>(Expression<Func<T,bool>> predicate) where T : MongoEntityBase
        {
            var qb = new QueryBuilder<T>();
            var query=qb.Where(predicate);
            var collection = GetCollection<T>();
            return collection.Find(query).ToList();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">The predicate.</param>
        public void Remove<T>(Expression<Func<T, bool>> predicate) where T : MongoEntityBase
        {
            var qb = new QueryBuilder<T>();
            var query = qb.Where(predicate);
            var collection = GetCollection<T>();
            var result = collection.Remove(query);
        }
    }
}