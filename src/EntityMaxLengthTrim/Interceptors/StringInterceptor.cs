// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.EntityMaxLengthTrim
//  Author           : RzR
//  Created On       : 2022-09-24 03:55
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-04-07 13:41
// ***********************************************************************
//  <copyright file="StringInterceptor.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Collections.Generic;
using System.Linq;
using EntityMaxLengthTrim.Extensions;
using EntityMaxLengthTrim.Options;

#endregion

namespace EntityMaxLengthTrim.Interceptors
{
    /// <summary>
    ///     String interceptor
    /// </summary>
    /// <remarks></remarks>
    public static class StringInterceptor
    {
        /// <summary>
        ///     Apply maximum allowed string length filter
        /// </summary>
        /// <param name="entity">Input entity</param>
        /// <param name="useDotOnEnd">
        ///     Optional. The default value is false.If set to <see langword="true" />, then at the end of
        ///     the string u will see (...); otherwise, string will be truncated.
        /// </param>
        /// <returns>Processed/parsed entity with new values</returns>
        /// <typeparam name="TEntity">Current entity type</typeparam>
        /// <remarks></remarks>
        public static TEntity ApplyStringMaxAllowedLength<TEntity>(TEntity entity, bool useDotOnEnd = false)
        {
            try
            {
                var entityType = typeof(TEntity);
                var stringProperties = entityType.GetStringPropertyInfos();
                foreach (var prop in stringProperties)
                {
                    var currentValue = (string)prop.GetValue(entity, null);
                    if (string.IsNullOrEmpty(currentValue)) continue;

                    var maxLength = prop.Name.GetMaxAllowedLength<TEntity>();

                    if (maxLength.IsNull()) continue;
                    if (!(currentValue.Length > maxLength)) continue;

                    var newValue = currentValue.Truncate(maxLength.Value, useDotOnEnd);
                    prop.SetValue(entity, newValue, null);
                }
            }
            catch
            {
                // ignored
                // In case of any error, return unmodified entity
            }

            return entity;
        }

        /// <summary>
        ///     Apply maximum allowed string length filter
        /// </summary>
        /// <param name="entity">Input entity</param>
        /// <param name="truncateWithDots">
        ///     List of properties where at the end of
        ///     the all string list u will see (...).
        /// </param>
        /// <param name="processOnlyAssigned">Process only properties from param 'truncateWithDots'</param>
        /// <returns>Processed/parsed entity with new values</returns>
        /// <typeparam name="TEntity">Current entity type</typeparam>
        /// <remarks></remarks>
        public static TEntity ApplyStringMaxAllowedLength<TEntity>(TEntity entity, IReadOnlyCollection<string> truncateWithDots,
            bool processOnlyAssigned = false)
        {
            try
            {
                var entityType = typeof(TEntity);
                var stringProperties = processOnlyAssigned.Equals(true)
                    ? entityType.GetStringPropertyInfos().Where(y => truncateWithDots.Contains(y.Name))
                    : entityType.GetStringPropertyInfos();
                foreach (var prop in stringProperties)
                {
                    var currentValue = (string)prop.GetValue(entity, null);
                    if (string.IsNullOrEmpty(currentValue)) continue;

                    var maxLength = prop.Name.GetMaxAllowedLength<TEntity>();

                    if (maxLength.IsNull()) continue;
                    if (!(currentValue.Length > maxLength)) continue;

                    var useWithDotTruncate = truncateWithDots?.Any(x => x == prop.Name) ?? false;
                    var newValue = currentValue.Truncate(maxLength.Value, useWithDotTruncate);
                    prop.SetValue(entity, newValue, null);
                }
            }
            catch
            {
                // ignored
                // In case of any error, return unmodified entity
            }

            return entity;
        }

        /// <summary>
        ///     Apply maximum allowed string length filter
        /// </summary>
        /// <param name="entity">Input entity</param>
        /// <param name="propertyName">Property name</param>
        /// <param name="useDots">
        ///     If set to <see langword="true" />, then property value at the end will have dots (...);
        ///     otherwise, value will be truncated at the allowed limit.
        /// </param>
        /// <returns>Processed/parsed entity with new values</returns>
        /// <typeparam name="TEntity">Current entity type</typeparam>
        /// <remarks></remarks>
        public static TEntity ApplyStringMaxAllowedLength<TEntity>(TEntity entity, string propertyName,
            bool useDots = true)
        {
            if (string.IsNullOrEmpty(propertyName)) return entity;

            try
            {
                var entityType = typeof(TEntity);
                var prop = entityType.GetStringPropertyInfos().FirstOrDefault(x => x.Name.Equals(propertyName));
                if (prop.IsNotNull())
                {
                    var currentValue = (string)prop!.GetValue(entity, null);
                    if (string.IsNullOrEmpty(currentValue)) return entity;

                    var maxLength = prop.Name.GetMaxAllowedLength<TEntity>();
                    if (maxLength.IsNull()) return entity;

                    if (!(currentValue.Length > maxLength)) return entity;

                    var newValue = currentValue.Truncate(maxLength.Value, useDots);
                    prop.SetValue(entity, newValue, null);
                }
            }
            catch
            {
                // ignored
                // In case of any error, return unmodified entity
            }

            return entity;
        }

        /// <summary>
        ///     Apply maximum allowed string length filter
        /// </summary>
        /// <param name="entity">Required. Input entity</param>
        /// <param name="options">Required. Option for processing fields</param>
        /// <param name="processOnlyAssigned">
        ///     Optional. The default value is false. If set to <see langword="true" />, then process
        ///     only properties from param 'truncateWithDots'; otherwise, will be processed all fields.
        /// </param>
        /// <returns>Processed/parsed entity with new values</returns>
        /// <typeparam name="TEntity">Current entity type</typeparam>
        /// <remarks></remarks>
        public static TEntity ApplyStringMaxAllowedLength<TEntity>(TEntity entity,
            IReadOnlyCollection<PropertyOption> options,
            bool processOnlyAssigned = false)
        {
            try
            {
                var entityType = typeof(TEntity);
                var stringProperties = processOnlyAssigned.Equals(true)
                    ? entityType.GetStringPropertyInfos().Where(y => options.Select(x => x.Name).Contains(y.Name))
                    : entityType.GetStringPropertyInfos();
                foreach (var prop in stringProperties)
                {
                    var currentValue = (string)prop.GetValue(entity, null);
                    if (string.IsNullOrEmpty(currentValue)) continue;

                    var maxLength = prop.Name.GetMaxAllowedLength<TEntity>();

                    if (maxLength.IsNull()) continue;
                    if (!(currentValue.Length > maxLength)) continue;

                    var useWithDotTruncate = options?.FirstOrDefault(x => x.Name == prop.Name)?.UseDots ?? false;
                    var newValue = currentValue.Truncate(maxLength.Value, useWithDotTruncate);
                    prop.SetValue(entity, newValue, null);
                }
            }
            catch
            {
                // ignored
                // In case of any error, return unmodified entity
            }

            return entity;
        }
        
        /// <summary>
        ///     Apply maximum allowed string length filter
        /// </summary>
        /// <param name="entity">Input entity</param>
        /// <param name="propertyName">Property name</param>
        /// <param name="useDots">
        ///     If set to <see langword="true" />, then property value at the end will have dots (...);
        ///     otherwise, value will be truncated at the allowed limit.
        /// </param>
        /// <returns>Processed/parsed property with new value</returns>
        /// <typeparam name="TEntity">Current entity type</typeparam>
        /// <remarks></remarks>
        public static string ApplyPropStringMaxAllowedLength<TEntity>(TEntity entity, string propertyName,
            bool useDots = true)
        {
            if (string.IsNullOrEmpty(propertyName)) return null;

            try
            {
                var entityType = typeof(TEntity);
                var prop = entityType.GetStringPropertyInfos().FirstOrDefault(x => x.Name.Equals(propertyName));
                if (prop.IsNotNull())
                {
                    var currentValue = (string)prop!.GetValue(entity, null);
                    if (string.IsNullOrEmpty(currentValue)) return currentValue;

                    var maxLength = prop.Name.GetMaxAllowedLength<TEntity>();
                    if (maxLength.IsNull()) return currentValue;
                    if (!(currentValue.Length > maxLength)) return currentValue;

                    var newValue = currentValue.Truncate(maxLength.Value, useDots);

                    return newValue;
                }
            }
            catch
            {
                // ignored
                // In case of any error, return unmodified property
            }

            return null;
        }
    }
}