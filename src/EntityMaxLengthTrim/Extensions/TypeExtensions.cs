// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.EntityMaxLengthTrim
//  Author           : RzR
//  Created On       : 2022-09-24 04:15
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-09-24 19:22
// ***********************************************************************
//  <copyright file="TypeExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#endregion

namespace EntityMaxLengthTrim.Extensions
{
    /// <summary>
    ///     Type extensions
    /// </summary>
    /// <remarks></remarks>
    internal static class TypeExtensions
    {
        /// <summary>
        ///     Get all properties
        /// </summary>
        /// <param name="type">Entity type</param>
        /// <returns></returns>
        internal static IEnumerable<PropertyInfo> GetPropertyInfos(this Type type)
        {
            try
            {
                return type
                    .GetProperties(BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public)
                    .ToList();
            }
            catch { return null; }
        }

        /// <summary>
        ///     Get all string properties
        /// </summary>
        /// <param name="type">Entity type</param>
        /// <returns></returns>
        internal static IEnumerable<PropertyInfo> GetStringPropertyInfos(this Type type)
        {
            try
            {
                var properties = GetPropertyInfos(type)?
                    .Where(x => x.PropertyType == typeof(string)).ToList();

                return properties;
            }
            catch { return null; }
        }

        /// <summary>
        ///     Get all string properties name
        /// </summary>
        /// <param name="type">Entity type</param>
        /// <returns></returns>
        internal static List<string> GetStringPropertyNames(this Type type)
        {
            try
            {
                var properties = GetStringPropertyInfos(type)?
                    .Where(x => x.PropertyType == typeof(string))
                    .Select(x => x.Name)
                    .ToList();

                return properties;
            }
            catch { return null; }
        }
    }
}