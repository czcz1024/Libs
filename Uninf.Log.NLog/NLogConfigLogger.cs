// ***********************************************************************
// Assembly         : Uninf.Log.NLog
// Author           : cz
// Created          : 02-03-2015
//
// Last Modified By : cz
// Last Modified On : 02-03-2015
// ***********************************************************************
// <copyright file="NLogConfigLogger.cs" company="Uninf">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.Log.NLog
{
    using global::NLog;
    using global::NLog.Config;
    using global::NLog.Targets;

    /// <summary>
    /// 代码配置的NLog日志类
    /// </summary>
    public class NLogConfigLogger:NLogLogger
    {
        /// <summary>
        /// NLog配置
        /// </summary>
        private NLogConfig config;

        /// <summary>
        /// Initializes a new instance of the <see cref="NLogConfigLogger"/> class.
        /// </summary>
        /// <param name="config">The configuration.</param>
        public NLogConfigLogger(NLogConfig config)
        {
            this.config = config;
        }

        /// <summary>
        /// 获取NLog的Logger对象，此方法实现了根据日志区分日志文件
        /// </summary>
        /// <returns>Logger.</returns>
        protected override global::NLog.Logger GetLogger()
        {
            if (log == null)
            {
                LoggingConfiguration config1 = new LoggingConfiguration { 
                   
                };
                var fileTarget = new FileTarget();
                fileTarget.Layout =config.Layout;

                fileTarget.FileName = config.FileSaveDir.TrimEnd('/')+"/${shortdate}.log";
                fileTarget.ArchiveFileName = "${basedir}/archives/log.{#}.txt";
                fileTarget.ArchiveEvery = FileArchivePeriod.Day;
                fileTarget.ArchiveNumbering = ArchiveNumberingMode.Rolling;
                fileTarget.KeepFileOpen = false;
                fileTarget.ConcurrentWrites = true;

                config1.AddTarget("file", fileTarget);
                config1.AddTarget("file",fileTarget);
                var rule = new LoggingRule("*", LogLevel.Debug, fileTarget);
                config1.LoggingRules.Add(rule);

                var fac = new LogFactory(config1);
                log = fac.GetLogger("default");
            }
            return log;
        }
    }
}