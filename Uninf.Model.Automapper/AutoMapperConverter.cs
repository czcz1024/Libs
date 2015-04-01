// ***********************************************************************
// Assembly         : Uninf.Model.Automapper
// Author           : cz
// Created          : 01-14-2015
//
// Last Modified By : cz
// Last Modified On : 01-14-2015
// ***********************************************************************
// <copyright file="AutoMapperConverter.cs" company="Uninf">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.Model.Automapper
{
    using AutoMapper;

    /// <summary>
    /// AutoMapperConverter类
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <typeparam name="TTarget">目标类型</typeparam>
    public class AutoMapperConverter<TSource,TTarget>:IModelConverter<TSource,TTarget>,IModelConverterRegister
    {

        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="source">源对象</param>
        /// <returns>目标对象</returns>
        public virtual TTarget Convert(TSource source)
        {
            return Mapper.Map<TSource, TTarget>(source);
        }

        /// <summary>
        /// 注册模型转换器
        /// 用于依赖注入批量注册
        /// </summary>
        public virtual void Regist()
        {
            Mapper.CreateMap<TSource, TTarget>();
        }
    }
}