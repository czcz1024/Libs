// ***********************************************************************
// Assembly         : Uninf.TimeTask.QuartZ
// Author           : cz
// Created          : 01-26-2015
//
// Last Modified By : cz
// Last Modified On : 01-29-2015
// ***********************************************************************
// <copyright file="QuartZServer.cs" company="Uninf">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.TimeTask.QuartZ
{
    using System.Collections.Generic;

    using Quartz;
    using Quartz.Impl;

    /// <summary>
    /// QuartZ定时任务服务管理
    /// </summary>
    public abstract class QuartZServer:ITimeTaskServer
    {
        /// <summary>
        /// 启动所有定时任务
        /// </summary>
        public virtual void StartAll()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            ConfigScheduler(scheduler);
            scheduler.Start();

            var jobs = GetJobs();
            foreach (var item in jobs)
            {
                scheduler.ScheduleJob(item.GetJob(), item.GetTrigger());
            }
        }

        /// <summary>
        /// 停止所有定时任务
        /// </summary>
        public virtual void StopAll()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Shutdown();
        }

        /// <summary>
        /// 获取所有任务
        /// </summary>
        /// <returns>IEnumerable&lt;IQuartZJob&gt;.</returns>
        protected abstract IEnumerable<IQuartZJob> GetJobs();

        /// <summary>
        /// 设置QuartZ Scheduler对象
        /// </summary>
        /// <param name="scheduler">The scheduler.</param>
        protected virtual void ConfigScheduler(IScheduler scheduler)
        {
        }
    }
}