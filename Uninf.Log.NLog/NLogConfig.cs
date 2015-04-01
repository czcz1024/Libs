// ***********************************************************************
// Assembly         : Uninf.Log.NLog
// Author           : cz
// Created          : 02-03-2015
//
// Last Modified By : cz
// Last Modified On : 02-03-2015
// ***********************************************************************
// <copyright file="NLogConfig.cs" company="Uninf">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.Log.NLog
{
    /// <summary>
    /// NLog配置类
    /// </summary>
    public class NLogConfig
    {
        /// <summary>
        /// 日志Layout
        /// </summary>
        /// <value>The layout.</value>
        public string Layout { get; set; }

        /// <summary>
        /// 保存路径
        /// </summary>
        /// <value>The file save dir.</value>
        public string FileSaveDir { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NLogConfig"/> class.
        /// </summary>
        public NLogConfig()
        {
            Layout = "${longdate} ${logger} ${message}";
        }
    }
}