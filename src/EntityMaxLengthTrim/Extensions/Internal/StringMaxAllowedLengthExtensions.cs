// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.EntityMaxLengthTrim
//  Author           : RzR
//  Created On       : 2022-09-23 08:38
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-09-24 03:34
// ***********************************************************************
//  <copyright file="StringMaxAllowedLengthExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using EntityMaxLengthTrim.Attributes;


#if DEBUG
#endif
#if NETSTANDARD1_5
using System.Reflection;
#endif

#endregion

namespace EntityMaxLengthTrim.Extensions.Internal
{
    /// <summary>
    ///     Maximum allowed length for string type extensions
    /// </summary>
    /// <remarks></remarks>
    internal static class StringMaxAllowedLengthExtensions
    {
        /// <summary>
        ///     Get the maximum allowed length for string property
        /// </summary>
        /// <param name="propertyName">Property name</param>
        /// <returns>Return property length decorated with DatAnnotation</returns>
        /// <remarks>Decoration allowed attributes: MaxLengthAttribute or StringLengthAttribute or MaxAllowedLengthAttribute</remarks>
        internal static int? GetMaxAllowedLength<TEntity>(this string propertyName)
            where TEntity : class
        {
            try
            {
                var length = propertyName.GetFromMaxLengthAttribute<TEntity>();
                if (length.IsNotNull())
                    return length;

                length = propertyName.GetFromStringLengthAttribute<TEntity>();
                if (length.IsNotNull())
                    return length;

                length = propertyName.GetFromMaxAllowedLengthAttribute<TEntity>();
                if (length.IsNotNull())
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
        /// <returns>Return property length decorated with MaxLengthAttribute</returns>
        /// <remarks>Return 'null' or '0' length in case of exception or not set property length value</remarks>
        private static int? GetFromMaxLengthAttribute<TEntity>(this string propertyName)
            where TEntity : class
        {
            try
            {
                var length = typeof(TEntity).GetProperty(propertyName)
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
        /// <returns>Return property length decorated with StringLengthAttribute</returns>
        /// <remarks>Return 'null' or '0' length in case of exception or not set property length value</remarks>
        private static int? GetFromStringLengthAttribute<TEntity>(this string propertyName)
            where TEntity : class
        {
            try
            {
                var length = typeof(TEntity).GetProperty(propertyName)
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
        /// <returns>Return property length decorated with MaxAllowedLengthAttribute</returns>
        /// <remarks>Return 'null' or '0' length in case of exception or not set property length value</remarks>
        private static int? GetFromMaxAllowedLengthAttribute<TEntity>(this string propertyName)
        where TEntity : class
        {
            try
            {
                var length = typeof(TEntity).GetProperty(propertyName)
                    ?.GetCustomAttributes(typeof(MaxAllowedLengthAttribute), false).Cast<MaxAllowedLengthAttribute>()
                    .FirstOrDefault();

                return length?.MaxLength;
            }
            catch { return null; }
        }
    }
}