// ***********************************************************************
// Assembly         : Uninf.Model.Automapper
// Author           : cz
// Created          : 01-09-2015
//
// Last Modified By : cz
// Last Modified On : 01-14-2015
// ***********************************************************************
// <copyright file="IModelConverterRegister.cs" company="Uninf">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.Model.Automapper
{
    /// <summary>
    /// IModelConverterRegister 接口
    /// </summary>
    public interface IModelConverterRegister
    {
        /// <summary>
        /// 注册模型转换器
        /// 用于依赖注入批量注册
        /// </summary>
        void Regist();
    }
}