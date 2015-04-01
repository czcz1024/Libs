// ***********************************************************************
// Assembly         : Uninf.Auth.CommonServiceLocator
// Author           : cz
// Created          : 01-08-2015
//
// Last Modified By : cz
// Last Modified On : 01-08-2015
// ***********************************************************************
// <copyright file="CommonServiceLocatorAuthContainer.cs" company="Uninf">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.Auth.CommonServiceLocator
{
    /// <summary>
    /// CommonServiceLocatorAuthContainer. 类
    /// </summary>
    public class CommonServiceLocatorAuthContainer:IAuthContainer
    {
        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>T.</returns>
        public T GetInstance<T>()
        {
            return Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<T>();
        }
    }
}