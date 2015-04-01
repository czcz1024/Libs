// ***********************************************************************
// Assembly         : Uninf.TimeTask.QuartZ
// Author           : cz
// Created          : 01-26-2015
//
// Last Modified By : cz
// Last Modified On : 01-29-2015
// ***********************************************************************
// <copyright file="QuartZTimeTaskBase.cs" company="Uninf">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.TimeTask.QuartZ
{
    using Quartz;

    /// <summary>
    /// Class QuartZTimeTaskBase.
    /// </summary>
    public abstract class QuartZTimeTaskBase : ITimeTask, IJob, IQuartZJob
    {
        /// <summary>
        /// 执行任务
        /// </summary>
        public abstract void Exec();

        /// <summary>
        /// 执行时间
        /// </summary>
        /// <returns>Corn表达式</returns>
        public abstract string RunTime();

        /// <summary>
        /// The job context
        /// </summary>
        protected IJobExecutionContext JobContext;
        /// <summary>
        /// Called by the <see cref="T:Quartz.IScheduler" /> when a <see cref="T:Quartz.ITrigger" />
        /// fires that is associated with the <see cref="T:Quartz.IJob" />.
        /// </summary>
        /// <param name="context">The execution context.</param>
        /// <remarks>The implementation may wish to set a  result object on the
        /// JobExecutionContext before this method exits.  The result itself
        /// is meaningless to Quartz, but may be informative to
        /// <see cref="T:Quartz.IJobListener" />s or
        /// <see cref="T:Quartz.ITriggerListener" />s that are watching the job's
        /// execution.</remarks>
        public virtual void Execute(IJobExecutionContext context)
        {
            JobContext = context;
            Exec();
        }

        /// <summary>
        /// 任务详情
        /// </summary>
        /// <returns>IJobDetail.</returns>
        public virtual IJobDetail GetJob()
        {
            return
                JobBuilder.Create(this.GetType()).WithIdentity(this.GetType().FullName, this.GetType().FullName).Build();
        }

        /// <summary>
        /// 触发器
        /// </summary>
        /// <returns>ITrigger.</returns>
        public virtual ITrigger GetTrigger()
        {
            return TriggerBuilder.Create()
                .WithIdentity(this.GetType().FullName, this.GetType().FullName)
                .WithCronSchedule(RunTime())
                .ForJob(this.GetType().FullName, this.GetType().FullName)
                .Build();
        }
    }
}