// ***********************************************************************
// Assembly         : Uninf.Bus
// Author           : cz
// Created          : 01-15-2015
//
// Last Modified By : cz
// Last Modified On : 01-15-2015
// ***********************************************************************
// <copyright file="JsonMessageSerializer.cs" company="Uninf">
//     Copyright ©  2014
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.Bus
{
    using Newtonsoft.Json;

    /// <summary>
    /// JsonMessageSerializer. 类
    /// Json序列化
    /// </summary>
    public class JsonMessageSerializer:IMessageSerializer
    {
        /// <summary>
        /// Serializes the specified MSG.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg">The MSG.</param>
        /// <returns>System.String.</returns>
        public string Serialize<T>(T msg)
        {
            return JsonConvert.SerializeObject(msg);
        }

        /// <summary>
        /// Deserializes the specified string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str">The string.</param>
        /// <returns>T.</returns>
        public T Deserialize<T>(string str)
        {
            return JsonConvert.DeserializeObject<T>(str);
        }
    }
}