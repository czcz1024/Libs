// ***********************************************************************
// Assembly         : Uninf.Auth
// Author           : cz
// Created          : 01-08-2015
//
// Last Modified By : cz
// Last Modified On : 01-09-2015
// ***********************************************************************
// <copyright file="IAuthContainer.cs" company="Uninf">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.Auth
{
    using System;

    /// <summary>
    /// IAuthContainer 接口
    /// </summary>
    public interface IAuthContainer
    {
        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>T.</returns>
        T GetInstance<T>();
    }

    /// <summary>
    /// AuthContainer. 类
    /// </summary>
    public class AuthContainer
    {
        /// <summary>
        /// Initializes static members of the <see cref="AuthContainer"/> class.
        /// </summary>
        static AuthContainer ()
        {
            Current = new DefaultAuthContainer();
        }

        /// <summary>
        /// Gets the current.
        /// </summary>
        /// <value>The current.</value>
        public static IAuthContainer Current { get; private set; }

        /// <summary>
        /// Sets the authentication container.
        /// </summary>
        /// <param name="container">The container.</param>
        public static void SetAuthContainer(IAuthContainer container)
        {
            Current = container;
        }
    }

    /// <summary>
    /// DefaultAuthContainer. 类
    /// </summary>
    public class DefaultAuthContainer : IAuthContainer
    {
        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>T.</returns>
        /// <exception cref="System.Exception">请使用AuthContainer.SetAuthContainer(...)设置依赖注入容器</exception>
        public T GetInstance<T>()
        {
            throw new Exception("请使用AuthContainer.SetAuthContainer(...)设置依赖注入容器");
        }
    }
}