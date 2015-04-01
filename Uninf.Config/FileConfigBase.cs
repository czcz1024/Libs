// ***********************************************************************
// Assembly         : Uninf.Config
// Author           : cz
// Created          : 01-08-2015
//
// Last Modified By : cz
// Last Modified On : 01-15-2015
// ***********************************************************************
// <copyright file="FileConfigBase.cs" company="Uninf">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.Config
{
    using System.IO;

    /// <summary>
    /// 基于文件的自定义配置基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class FileTHZConfigBase<T> : THZConfigBase<T> where T:THZConfigBase<T>
    {
        /// <summary>
        /// 保存路径
        /// </summary>
        /// <returns>System.String.</returns>
        public abstract string FilePath();
        /// <summary>
        /// 默认值保存路径
        /// </summary>
        /// <returns>System.String.</returns>
        public abstract string DefaultFilePath();
        /// <summary>
        /// 序列化方式
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>System.String.</returns>
        public abstract string Serialize(T obj);
        /// <summary>
        /// 反序列化方式
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns>T.</returns>
        public abstract T Deserialize(string str);

        /// <summary>
        /// 存在配置文件
        /// </summary>
        /// <returns><c>true</c> if this instance has configuration; otherwise, <c>false</c>.</returns>
        public override bool HasConfig()
        {
            return File.Exists(this.FilePath());
        }

        /// <summary>
        /// 从文件加载
        /// </summary>
        /// <returns>T.</returns>
        public override T Load()
        {
            var str = File.ReadAllText(this.FilePath());
            var r= this.Deserialize(str);
            return r;
        }


        /// <summary>
        /// 保存到文件
        /// </summary>
        /// <param name="obj">The object.</param>
        public override void Save(T obj)
        {
            var fileInfo = new FileInfo(this.FilePath());
            if (!fileInfo.Directory.Exists)
            {
                fileInfo.Directory.Create();
            }
            File.WriteAllText(this.FilePath(), this.Serialize(obj));
        }

        /// <summary>
        /// 保存默认
        /// </summary>
        /// <param name="obj">The object.</param>
        public override void SaveDefault(T obj)
        {
            var fileInfo = new FileInfo(this.DefaultFilePath());
            if (!fileInfo.Directory.Exists)
            {
                fileInfo.Directory.Create();
            }
            File.WriteAllText(this.DefaultFilePath(), this.Serialize(obj));
        }

    }
}