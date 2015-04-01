// ***********************************************************************
// Assembly         : Uninf.Config.PageTemplate
// Author           : cz
// Created          : 01-08-2015
//
// Last Modified By : cz
// Last Modified On : 01-08-2015
// ***********************************************************************
// <copyright file="ConfigExtension.cs" company="Uninf">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.Config.PageTemplate
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// ConfigExtension. 类
    /// </summary>
    public static class ConfigExtension
    {
        /// <summary>
        /// Gets the display name.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>System.String.</returns>
        public static string GetDisplayName(this PropertyInfo property)
        {
            var name = property.Name;
            var attr = property.GetCustomAttributes(typeof(DisplayNameAttribute), false).FirstOrDefault();
            if (attr != null)
            {
                name = ((DisplayNameAttribute)attr).DisplayName;
            }
            return name;
        }

        /// <summary>
        /// Determines whether [is simple type] [the specified type].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if [is simple type] [the specified type]; otherwise, <c>false</c>.</returns>
        public static bool IsSimpleType(this Type type)
        {
            if (type == typeof(DateTime)) return true;
            if (type == typeof(string)) return true;
            return type.IsPrimitive;
        }

        /// <summary>
        /// Determines whether the specified type is enumerable.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if the specified type is enumerable; otherwise, <c>false</c>.</returns>
        public static bool IsEnumerable(this Type type)
        {
            return type.GetInterfaces().Any(x => x == typeof(IEnumerable));
        }

        /// <summary>
        /// Gets the type of the data.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>DataType.</returns>
        public static DataType GetDataType(this PropertyInfo property)
        {
            var att=property.GetCustomAttributes(typeof(DataTypeAttribute), false).FirstOrDefault();
            if(att!=null)
                return ((DataTypeAttribute)att).DataType;
            return DataType.Text;
        }
    }
}