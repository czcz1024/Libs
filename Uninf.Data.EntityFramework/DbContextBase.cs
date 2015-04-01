// ***********************************************************************
// Assembly         : Uninf.Data.EntityFramework
// Author           : cz
// Created          : 12-26-2014
//
// Last Modified By : cz
// Last Modified On : 01-12-2015
// ***********************************************************************
// <copyright file="DbContextBase.cs" company="Uninf">
//     Copyright ©  2014
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.Data.EntityFramework
{
    using System.Data.Common;
    using System.Data.Entity;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Infrastructure;

    /// <summary>
    /// DbContextBase. 类
    /// </summary>
    public abstract class DbContextBase:DbContext,IUnitOfWork
    {
        /// <summary>
        /// Constructs a new context instance using the given string as the name or connection string for the
        /// database to which a connection will be made.
        /// See the class remarks for how this is used to create a connection.
        /// </summary>
        /// <param name="nameOrConnectionString">Either the database name or a connection string.</param>
        protected DbContextBase(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        /// <summary>
        /// Constructs a new context instance using the given string as the name or connection string for the
        /// database to which a connection will be made, and initializes it from the given model.
        /// See the class remarks for how this is used to create a connection.
        /// </summary>
        /// <param name="nameOrConnectionString">Either the database name or a connection string.</param>
        /// <param name="model">The model that will back this context.</param>
        protected DbContextBase(string nameOrConnectionString, DbCompiledModel model)
            : base(nameOrConnectionString, model)
        {
        }

        /// <summary>
        /// Constructs a new context instance using the existing connection to connect to a database.
        /// The connection will not be disposed when the context is disposed if <paramref name="contextOwnsConnection" />
        /// is <c>false</c>.
        /// </summary>
        /// <param name="existingConnection">An existing connection to use for the new context.</param>
        /// <param name="contextOwnsConnection">If set to <c>true</c> the connection is disposed when the context is disposed, otherwise the caller must dispose the connection.</param>
        protected DbContextBase(DbConnection existingConnection, bool contextOwnsConnection)
            : base(existingConnection, contextOwnsConnection)
        {
        }

        /// <summary>
        /// Constructs a new context instance using the existing connection to connect to a database,
        /// and initializes it from the given model.
        /// The connection will not be disposed when the context is disposed if <paramref name="contextOwnsConnection" />
        /// is <c>false</c>.
        /// </summary>
        /// <param name="existingConnection">An existing connection to use for the new context.</param>
        /// <param name="model">The model that will back this context.</param>
        /// <param name="contextOwnsConnection">If set to <c>true</c> the connection is disposed when the context is disposed, otherwise the caller must dispose the connection.</param>
        protected DbContextBase(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection)
            : base(existingConnection, model, contextOwnsConnection)
        {
        }

        /// <summary>
        /// Constructs a new context instance around an existing ObjectContext.
        /// </summary>
        /// <param name="objectContext">An existing ObjectContext to wrap with the new context.</param>
        /// <param name="dbContextOwnsObjectContext">If set to <c>true</c> the ObjectContext is disposed when the DbContext is disposed, otherwise the caller must dispose the connection.</param>
        protected DbContextBase(ObjectContext objectContext, bool dbContextOwnsObjectContext)
            : base(objectContext, dbContextOwnsObjectContext)
        {
        }

        /// <summary>
        /// Constructs a new context instance using conventions to create the name of the database to
        /// which a connection will be made, and initializes it from the given model.
        /// The by-convention name is the full name (namespace + class name) of the derived context class.
        /// See the class remarks for how this is used to create a connection.
        /// </summary>
        /// <param name="model">The model that will back this context.</param>
        protected DbContextBase(DbCompiledModel model)
            : base(model)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbContextBase"/> class.
        /// </summary>
        protected DbContextBase()
        {
        }

        /// <summary>
        /// 获取仓储
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <returns>IRepository&lt;T&gt;.</returns>
        public IRepository<T> GetRepository<T>() where T:class
        {
            return new RepositoryEfAdapter<T>(Set<T>(), this);
        }
    }
}