// ***********************************************************************
// Assembly         : Uninf.Config
// Author           : cz
// Created          : 01-08-2015
//
// Last Modified By : cz
// Last Modified On : 01-15-2015
// ***********************************************************************
// <copyright file="THZConfigHelper.cs" company="Uninf">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.Config
{
    using System;

    /// <summary>
    /// THZConfigHelper. 类
    /// 读取配置请用此类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class THZConfigHelper<T> where T : THZConfigBase<T>
    {
        /// <summary>
        /// The _instance
        /// </summary>
        static T _instance;

        /// <summary>
        /// 访问实例
        /// </summary>
        /// <value>The instance.</value>
        public static T Instance
        {
            get { return _instance ?? (_instance = Get()); }
        }

        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns>T.</returns>
        private static T Get()
        {
            var gen = THZConfigBase<T>.Instance;
            if (gen.HasConfig())
            {
                var r= gen.Load();
                return r;
            }
            var obj = gen.Create();
            gen.Save(obj);
            gen.SaveDefault(obj);
            return obj;
        }

        /// <summary>
        /// Reloads this instance.
        /// </summary>
        /// <exception cref="System.Exception">未找到配置文件</exception>
        public static void Reload()
        {
            var gen = THZConfigBase<T>.Instance;
            if (gen.HasConfig())
            {
                _instance=gen.Load();
                return;
            }
            throw new Exception("未找到配置文件");
        }
    }
}