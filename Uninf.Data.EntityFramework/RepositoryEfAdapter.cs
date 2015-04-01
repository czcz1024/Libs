// ***********************************************************************
// Assembly         : Uninf.Data.EntityFramework
// Author           : cz
// Created          : 01-07-2015
//
// Last Modified By : cz
// Last Modified On : 02-27-2015
// ***********************************************************************
// <copyright file="RepositoryEfAdapter.cs" company="Uninf">
//     Copyright ©  2014
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.Data.EntityFramework
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// 仓储与EF适配类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RepositoryEfAdapter<T> : IRepository<T> where T : class
    {
        /// <summary>
        /// The DbSet
        /// </summary>
        private DbSet<T> set;

        /// <summary>
        /// The DbContext
        /// </summary>
        private DbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryEfAdapter{T}"/> class.
        /// </summary>
        /// <param name="set">The set.</param>
        /// <param name="context">The context.</param>
        public RepositoryEfAdapter(DbSet<T> set, DbContext context)
        {
            this.set = set;
            this.context = context;
        }

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>T.</returns>
        public T Add(T entity)
        {
            return this.set.Add(entity);
        }

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>T.</returns>
        public T Delete(T entity)
        {
            var entry = context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                set.Attach(entity);
            }
            return this.set.Remove(entity);
        }

        /// <summary>
        /// Finds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>T.</returns>
        public T Find(params object[] key)
        {
            return this.set.Find(key);
        }

        /// <summary>
        /// Ases the queryable.
        /// </summary>
        /// <returns>IQueryable&lt;T&gt;.</returns>
        public IQueryable<T> AsQueryable()
        {
            return this.set;
        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>T.</returns>
        public T Update(T entity)
        {
            var entry = this.context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                this.set.Attach(entity);
                entry.State = EntityState.Modified;
            }
            return entity;
        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="updateProperties">The update properties.</param>
        /// <returns>T.</returns>
        public T Update(T entity, params Expression<Func<T, object>>[] updateProperties)
        {
            if (!updateProperties.Any())
            {
                return this.Update(entity);
            }
            var entry = this.context.Entry(entity);
            foreach (var property in updateProperties)
            {
                entry.Property(property).IsModified = true;
            }
            return entity;
        }
    }
}