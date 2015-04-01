// ***********************************************************************
// Assembly         : Uninf.Data.Mongo
// Author           : cz
// Created          : 01-20-2015
//
// Last Modified By : cz
// Last Modified On : 01-20-2015
// ***********************************************************************
// <copyright file="MongoEntityBase.cs" company="Uninf">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.Data.Mongo
{
    using MongoDB.Bson;

    /// <summary>
    /// MongoEntityBase. 类
    /// 所有需要放入mongo的对象比较继承自此类
    /// </summary>
    public class MongoEntityBase
    {
        /// <summary>
        /// id属性
        /// </summary>
        /// <value>The identifier.</value>
        public ObjectId Id { get; set; }
    }
}