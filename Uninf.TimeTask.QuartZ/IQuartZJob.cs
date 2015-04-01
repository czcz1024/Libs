// ***********************************************************************
// Assembly         : Uninf.TimeTask.QuartZ
// Author           : cz
// Created          : 01-26-2015
//
// Last Modified By : cz
// Last Modified On : 02-27-2015
// ***********************************************************************
// <copyright file="IQuartZJob.cs" company="Uninf">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.TimeTask.QuartZ
{
    using Quartz;

    /// <summary>
    /// QuartZ定时任务接口
    /// </summary>
    public interface IQuartZJob
    {
        /// <summary>
        /// 任务详情
        /// </summary>
        /// <returns>IJobDetail.</returns>
        IJobDetail GetJob();

        /// <summary>
        /// 触发器
        /// </summary>
        /// <returns>ITrigger.</returns>
        ITrigger GetTrigger();
    }
}