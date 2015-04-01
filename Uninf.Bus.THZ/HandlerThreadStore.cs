// ***********************************************************************
// Assembly         : Uninf.Bus.THZ
// Author           : cz
// Created          : 01-08-2015
//
// Last Modified By : cz
// Last Modified On : 01-27-2015
// ***********************************************************************
// <copyright file="HandlerThreadStore.cs" company="Uninf">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.Bus.THZ
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using Uninf.Bus.RabbitMq;

    /// <summary>
    /// HandlerThreadStore. 类
    /// 监听线程保存实现
    /// </summary>
    public class HandlerThreadStore : IHandlerThreadStore
    {
        /// <summary>
        /// The dic
        /// </summary>
        public static ConcurrentDictionary<Type, List<Tuple<int, object>>> Dic
            = new ConcurrentDictionary<Type, List<Tuple<int, object>>>();

        /// <summary>
        /// Adds the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="threadid">The threadid.</param>
        /// <param name="c">The c.</param>
        public void Add(Type type, int threadid, object c)
        {
            var list = Dic.GetOrAdd(type, new List<Tuple<int, object>>());
            list.Add(new Tuple<int, object>(threadid, c));
        }

        /// <summary>
        /// Deletes the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="threadid">The threadid.</param>
        public void Delete(Type type, int threadid)
        {
            List<Tuple<int, object>> list;
            HandlerThreadStore.Dic.TryGetValue(type, out list);
            if (list != null)
            {
                list.RemoveAll(x => x.Item1 == threadid);
            }
        }

        /// <summary>
        /// Gets the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="threadid">The threadid.</param>
        /// <returns>System.Object.</returns>
        public object Get(Type type, int threadid)
        {
            List<Tuple<int, object>> list;
            HandlerThreadStore.Dic.TryGetValue(type, out list);
            if (list != null)
            {
                var first = list.FirstOrDefault(x => x.Item1 == threadid);
                if (first != null)
                {
                    return first.Item2;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>IDictionary&lt;Type, List&lt;Tuple&lt;System.Int32, System.Object&gt;&gt;&gt;.</returns>
        public IDictionary<Type, List<Tuple<int, object>>> GetAll()
        {
            return Dic;
        }
    }
}