// ***********************************************************************
// Assembly         : Uninf.Config
// Author           : cz
// Created          : 01-08-2015
//
// Last Modified By : cz
// Last Modified On : 01-08-2015
// ***********************************************************************
// <copyright file="Singleton.cs" company="Uninf">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.Config
{
    using System;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// 单例模式
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Singleton<T>
    {
        /// <summary>
        /// The instance
        /// </summary>
        private static Lazy<T> instance
          = new Lazy<T>(() =>
          {
              var ctors = typeof(T).GetConstructors( 
                  BindingFlags.Instance|
                  BindingFlags.NonPublic|
                  BindingFlags.Public);


              if (ctors.Count() != 1)
                  throw new InvalidOperationException(String.Format("类型 {0} 必须有一个构造函数", typeof(T)));

              var ctor = ctors.SingleOrDefault(c => !c.GetParameters().Any() && c.IsPrivate);

              if (ctor == null)
                  throw new InvalidOperationException(String.Format("类型 {0} 的构造函数必须是privte并且无参", typeof(T)));

              return (T)ctor.Invoke(null);
          });



        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        internal static T Instance
        {
            get { return instance.Value; }
        }
    }
}