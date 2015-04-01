// ***********************************************************************
// Assembly         : Uninf.Bus
// Author           : cz
// Created          : 01-15-2015
//
// Last Modified By : cz
// Last Modified On : 01-27-2015
// ***********************************************************************
// <copyright file="IHandlerThreadStore.cs" company="Uninf">
//     Copyright ©  2014
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.Bus
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    /// <summary>
    /// IHandlerThreadStore 接口
    /// 监听线程存储接口
    /// </summary>
    public interface IHandlerThreadStore
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="type">处理器类型</param>
        /// <param name="threadid">线程id</param>
        /// <param name="c">线程cancel对象</param>
        void Add(Type type,int threadid, object c);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="type">处理器类型</param>
        /// <param name="threadid">线程id</param>
        void Delete(Type type,int threadid);

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="type">处理器类型</param>
        /// <param name="threadid">线程id</param>
        /// <returns>System.Object.</returns>
        object Get(Type type, int threadid);

        /// <summary>
        /// 全部
        /// </summary>
        /// <returns>IDictionary&lt;Type, List&lt;Tuple&lt;System.Int32, System.Object&gt;&gt;&gt;.</returns>
        IDictionary<Type, List<Tuple<int, object>>> GetAll();
    }
}