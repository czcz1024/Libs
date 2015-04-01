// ***********************************************************************
// Assembly         : Uninf.Model
// Author           : cz
// Created          : 01-09-2015
//
// Last Modified By : cz
// Last Modified On : 01-14-2015
// ***********************************************************************
// <copyright file="IModelConverter.cs" company="Uninf">
//     Copyright ©  2014
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.Model
{
    /// <summary>
    /// IModelConverter 接口
    /// 模型转换，用于批量依赖注入
    /// </summary>
    public interface IModelConverter
    {
    }

    /// <summary>
    /// IModelConverter 接口
    /// 模型转换
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <typeparam name="TTarget">目标类型</typeparam>
    public interface IModelConverter<in TSource, out TTarget>:IModelConverter
    {
        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="source">源对象</param>
        /// <returns>目标对象</returns>
        TTarget Convert(TSource source);
    }
}