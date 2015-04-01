// ***********************************************************************
// Assembly         : Uninf.Config
// Author           : cz
// Created          : 01-08-2015
//
// Last Modified By : cz
// Last Modified On : 01-08-2015
// ***********************************************************************
// <copyright file="XmlConfigExtension.cs" company="Uninf">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.Config
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// XmlConfigExtension. 类
    /// </summary>
    internal static class XmlConfigExtension
    {
        /// <summary>
        /// Gets the expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src">The source.</param>
        /// <returns>Expression&lt;Func&lt;T, System.Object&gt;&gt;.</returns>
        public static Expression<Func<T, object>> GetExpression<T>(string src) where T:XmlFileConfigBase<T>
        {
            var props = src.Split('.');
            var baseParam = Expression.Parameter(typeof(T), "x");
            MemberExpression mem = Expression.Property(baseParam, props[0]);
            for (var i = 1; i < props.Length; i++)
            {
                mem = Expression.Property(mem, props[i]);

            }
            var exp = Expression.Lambda<Func<T, object>>(Expression.Convert(mem, typeof(object)), baseParam);
            return exp;
        }
    }
}