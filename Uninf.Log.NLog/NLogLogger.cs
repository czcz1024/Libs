// ***********************************************************************
// Assembly         : Uninf.Log.NLog
// Author           : cz
// Created          : 02-03-2015
//
// Last Modified By : cz
// Last Modified On : 02-03-2015
// ***********************************************************************
// <copyright file="NLogLogger.cs" company="Uninf">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uninf.Log.NLog
{
    using global::NLog;

    /// <summary>
    /// NLog日志类
    /// </summary>
    public class NLogLogger:ILogger
    {
        /// <summary>
        /// NLog Logger对象
        /// </summary>
        protected global::NLog.Logger log;
        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="msg">信息</param>
        public void Error(string msg)
        {
            GetLogger().Error(msg);
        }

        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="msg">信息</param>
        public void Debug(string msg)
        {
            GetLogger().Debug(msg);
        }

        /// <summary>
        /// 记录
        /// </summary>
        /// <param name="msg">信息</param>
        public void Info(string msg)
        {
            GetLogger().Info(msg);
        }

        /// <summary>
        /// 获取NLog的Logger对象
        /// </summary>
        /// <returns>Logger.</returns>
        protected virtual Logger GetLogger()
        {
            if (log == null)
            {
                var fac = new LogFactory();
                log=fac.GetLogger("default");
            }
            return log;
        }
    }
}
