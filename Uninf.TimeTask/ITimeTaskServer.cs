// ***********************************************************************
// Assembly         : Uninf.TimeTask
// Author           : cz
// Created          : 01-26-2015
//
// Last Modified By : cz
// Last Modified On : 02-27-2015
// ***********************************************************************
// <copyright file="ITimeTaskServer.cs" company="Uninf">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.TimeTask
{
    /// <summary>
    /// 定时任务服务管理接口
    /// </summary>
    public interface ITimeTaskServer
    {
        /// <summary>
        /// 启动所有定时任务
        /// </summary>
        void StartAll();

        /// <summary>
        /// 停止所有定时任务
        /// </summary>
        void StopAll();
    }
}