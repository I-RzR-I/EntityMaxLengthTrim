// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.EntityMaxLengthTrim
//  Author           : RzR
//  Created On       : 2022-09-23 08:38
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-09-24 03:34
// ***********************************************************************
//  <copyright file="StringMaxAllowedLengthHelper.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

#if DEBUG
using System;
using System.Diagnostics;
#endif
using System.ComponentModel.DataAnnotations;
#if NETSTANDARD1_5
using System.Reflection;
#endif
using System.Linq;
using EntityMaxLengthTrim.Attributes;

#endregion

namespace EntityMaxLengthTrim.Helpers
{
    /// <summary>
    ///     Maximum allowed length for string type helper
    /// </summary>
    /// <remarks></remarks>
    internal static class StringMaxAllowedLengthHelper
    {
        /// <summary>
        ///     Get the maximum allowed length for string property
        /// </summary>
        /// <param name="propertyName">Property name</param>
        /// <returns></returns>
        /// <remarks></remarks>
        internal static int? GetMaxAllowedLength<T>(this string propertyName)
        {
            try
            {
                var length = propertyName.GetFromMaxLengthAttribute<T>();
                if (length != null)
                    return length;

                length = propertyName.GetFromStringLengthAttribute<T>();
                if (length != null)
                    return length;

                length = propertyName.GetFromMaxAllowedLengthAttribute<T>();
                if (length != null)
                    return length;
            }
#if DEBUG
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
#else
            catch
            {
#endif
                return null;
            }

            return null;
        }

        /// <summary>
        ///     Get max length from 'MaxLength' attribute
        /// </summary>
        /// <param name="propertyName">Property name</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static int? GetFromMaxLengthAttribute<T>(this string propertyName)
        {
            try
            {
                var length = typeof(T).GetProperty(propertyName)
                    ?.GetCustomAttributes(typeof(MaxLengthAttribute), false).Cast<MaxLengthAttribute>()
                    .FirstOrDefault();

                return length?.Length;
            }
            catch { return null; }
        }

        /// <summary>
        ///     Get max length from 'StringLength' attribute
        /// </summary>
        /// <param name="propertyName">Property name</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static int? GetFromStringLengthAttribute<T>(this string propertyName)
        {
            try
            {
                var length = typeof(T).GetProperty(propertyName)
                    ?.GetCustomAttributes(typeof(StringLengthAttribute), false).Cast<StringLengthAttribute>()
                    .FirstOrDefault();

                return length?.MaximumLength;
            }
            catch { return null; }
        }
        
        /// <summary>
        ///     Get max length from 'MaxAllowedLength' attribute
        /// </summary>
        /// <param name="propertyName">Property name</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static int? GetFromMaxAllowedLengthAttribute<T>(this string propertyName)
        {
            try
            {
                var length = typeof(T).GetProperty(propertyName)
                    ?.GetCustomAttributes(typeof(MaxAllowedLengthAttribute), false).Cast<MaxAllowedLengthAttribute>()
                    .FirstOrDefault();

                return length?.MaxLength;
            }
            catch { return null; }
        }
    }
}