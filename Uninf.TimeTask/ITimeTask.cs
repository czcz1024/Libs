// ***********************************************************************
// Assembly         : Uninf.TimeTask
// Author           : cz
// Created          : 01-26-2015
//
// Last Modified By : cz
// Last Modified On : 02-27-2015
// ***********************************************************************
// <copyright file="ITimeTask.cs" company="Uninf">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.TimeTask
{
    /// <summary>
    /// 定时任务接口
    /// </summary>
    public interface ITimeTask
    {
        /// <summary>
        /// 执行任务
        /// </summary>
        void Exec();

        /// <summary>
        /// 执行时间
        /// </summary>
        /// <returns>Corn表达式</returns>
        string RunTime();
    }
}