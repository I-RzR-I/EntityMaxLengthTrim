// ***********************************************************************
//  Assembly         : RzR.Extensions.EntityLength
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
using System.Linq;
using System.Reflection;
using RzR.Extensions.EntityLength.Attributes;

#if DEBUG
using System.Diagnostics;
#endif

#endregion

namespace RzR.Extensions.EntityLength.Extensions.Internal
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
                var propertyInfo = typeof(TEntity).GetProperty(propertyName);

                return propertyInfo.GetMaxAllowedLength();
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
        }

        /// <summary>
        ///     Get the maximum allowed length for a string property.
        /// </summary>
        /// <param name="propertyInfo">Property information.</param>
        /// <returns>Resolved max length from configured attributes.</returns>
        internal static int? GetMaxAllowedLength(this PropertyInfo propertyInfo)
        {
            try
            {
                if (propertyInfo.IsNull())
                    return null;

                var maxLength = propertyInfo.GetCustomAttributes(typeof(MaxLengthAttribute), false)
                    .Cast<MaxLengthAttribute>()
                    .FirstOrDefault();
                if (maxLength.IsNotNull())
                    return maxLength!.Length;

                var stringLength = propertyInfo.GetCustomAttributes(typeof(StringLengthAttribute), false)
                    .Cast<StringLengthAttribute>()
                    .FirstOrDefault();
                if (stringLength.IsNotNull())
                    return stringLength!.MaximumLength;

                var customLength = propertyInfo.GetCustomAttributes(typeof(MaxAllowedLengthAttribute), false)
                    .Cast<MaxAllowedLengthAttribute>()
                    .FirstOrDefault();
                if (customLength.IsNotNull())
                    return customLength!.MaxLength;
            }
            catch { return null; }

            return null;
        }
    }
}