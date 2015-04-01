// ***********************************************************************
// Assembly         : Uninf.Bus
// Author           : cz
// Created          : 01-09-2015
//
// Last Modified By : cz
// Last Modified On : 01-09-2015
// ***********************************************************************
// <copyright file="IMessageSerializer.cs" company="Uninf">
//     Copyright ©  2014
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.Bus
{
    /// <summary>
    /// IMessageSerializer 接口
    /// 消息内容序列化接口
    /// </summary>
    public interface IMessageSerializer
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg">The MSG.</param>
        /// <returns>System.String.</returns>
        string Serialize<T>(T msg);

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str">The string.</param>
        /// <returns>T.</returns>
        T Deserialize<T>(string str);
    }
}