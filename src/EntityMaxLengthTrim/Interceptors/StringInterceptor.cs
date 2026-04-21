// ***********************************************************************
//  Assembly         : RzR.Extensions.EntityLength
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

using System;
using System.Collections.Generic;
using System.Linq;
using RzR.Extensions.EntityLength.Enums;
using RzR.Extensions.EntityLength.Exceptions;
using RzR.Extensions.EntityLength.Extensions.Internal;
using RzR.Extensions.EntityLength.Interceptors.Internal;
using RzR.Extensions.EntityLength.Options;

#endregion

namespace RzR.Extensions.EntityLength.Interceptors
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
        /// <param name="truncateType">
        ///     (Optional) Type of the truncate.
        ///     Truncate string from the beginning or at the end.
        /// </param>
        /// <returns>Processed/parsed entity with new values</returns>
        /// <typeparam name="TEntity">Current entity type</typeparam>
        /// <remarks></remarks>
        public static TEntity ApplyStringMaxAllowedLength<TEntity>(
            TEntity entity,
            bool useDotOnEnd = false,
            StringTruncateType truncateType = StringTruncateType.AtTheEndOf)
            where TEntity : class
        {
            if (entity.IsNull()) throw new ArgumentNullException(nameof(entity));

            var entityType = typeof(TEntity);
            var stringProperties = EntityStringPropertyCache.GetStringProperties(entityType);
            foreach (var stringProperty in stringProperties)
            {
                var currentValue = stringProperty.GetValue(entity);
                if (!currentValue.IsPresent()) continue;

                var maxLength = stringProperty.MaxLength;

                if (maxLength.IsNull()) continue;
                if (!(currentValue.Length > maxLength)) continue;

                var newValue = truncateType == StringTruncateType.AtTheEndOf
                    ? currentValue.Truncate(maxLength.Value, useDotOnEnd)
                    : currentValue.TruncateAtStart(maxLength.Value, useDotOnEnd);

                stringProperty.SetValue(entity, newValue);
            }

            return entity;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Apply maximum allowed string length filter.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <exception cref="EntityMaxLengthExceededException">
        ///     Thrown when <see cref="TrimOption.Policy" /> is <see cref="TrimPolicy.Throw" /> and a string
        ///     property exceeds its configured maximum length. The exception carries the
        ///     <see cref="TrimContext" /> describing the offending property.
        /// </exception>
        /// <typeparam name="TEntity">Type of the entity.</typeparam>
        /// <param name="entity">Input entity.</param>
        /// <param name="trimOption">The trim option.</param>
        /// <returns>
        ///     Processed/parsed entity with new values.
        /// </returns>
        /// =================================================================================================
        public static TEntity ApplyStringMaxAllowedLength<TEntity>(
            TEntity entity,
            TrimOption trimOption)
            where TEntity : class
        {
            if (entity.IsNull())
                throw new ArgumentNullException(nameof(entity));
            if (trimOption.IsNull())
                trimOption = new TrimOption();

            var entityType = typeof(TEntity);
            var stringProperties = EntityStringPropertyCache.GetStringProperties(entityType);
            foreach (var stringProperty in stringProperties)
            {
                var currentValue = stringProperty.GetValue(entity);
                if (!currentValue.IsPresent()) continue;

                var maxLength = stringProperty.MaxLength;

                if (maxLength.IsNull()) continue;
                if (!(currentValue.Length > maxLength)) continue;

                // For TrimPolicy.Throw, fail fast before doing any truncation work and surface a
                // typed exception that carries the full TrimContext.
                if (trimOption.Policy == TrimPolicy.Throw)
                {
                    var throwCtx = new TrimContext(entityType.Name, stringProperty.Name, currentValue, currentValue, maxLength);
                    throw new EntityMaxLengthExceededException(throwCtx);
                }

                var newValue = trimOption.TruncateType == StringTruncateType.AtTheEndOf
                    ? currentValue.Truncate(maxLength.Value, trimOption.UseDots)
                    : currentValue.TruncateAtStart(maxLength.Value, trimOption.UseDots);

                var ctx = new TrimContext(entityType.Name, stringProperty.Name, currentValue, newValue, maxLength);

                switch (trimOption.Policy)
                {
                    case TrimPolicy.Silent:
                        stringProperty.SetValue(entity, newValue);
                        break;
                    case TrimPolicy.Warn:
                        trimOption.Logger?.Invoke(ctx);
                        stringProperty.SetValue(entity, newValue);
                        break;
                    case TrimPolicy.ReportOnly:
                        trimOption.Logger?.Invoke(ctx);
                        break;
                    default:
                        stringProperty.SetValue(entity, newValue);
                        break;
                }
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
        /// <param name="truncateType">
        ///     (Optional) Type of the truncate.
        ///     Truncate string from the beginning or at the end.
        /// </param>
        /// <returns>Processed/parsed entity with new values</returns>
        /// <typeparam name="TEntity">Current entity type</typeparam>
        /// <remarks></remarks>
        public static TEntity ApplyStringMaxAllowedLength<TEntity>(
            TEntity entity,
            IReadOnlyCollection<string> truncateWithDots,
            bool processOnlyAssigned = false,
            StringTruncateType truncateType = StringTruncateType.AtTheEndOf)
            where TEntity : class
        {
            if (entity.IsNull()) 
                throw new ArgumentNullException(nameof(entity));
            if (truncateWithDots.IsNull()) 
                throw new ArgumentNullException(nameof(truncateWithDots));

            var entityType = typeof(TEntity);
            var propertiesWithDots = new HashSet<string>(truncateWithDots, StringComparer.Ordinal);
            var stringProperties = EntityStringPropertyCache.GetStringProperties(entityType);
            foreach (var stringProperty in stringProperties)
            {
                if (processOnlyAssigned && !propertiesWithDots.Contains(stringProperty.Name))
                    continue;

                var currentValue = stringProperty.GetValue(entity);
                if (!currentValue.IsPresent()) continue;

                var maxLength = stringProperty.MaxLength;

                if (maxLength.IsNull()) continue;
                if (!(currentValue.Length > maxLength)) continue;

                var useWithDotTruncate = propertiesWithDots.Contains(stringProperty.Name);
                var newValue = truncateType == StringTruncateType.AtTheEndOf
                    ? currentValue.Truncate(maxLength.Value, useWithDotTruncate)
                    : currentValue.TruncateAtStart(maxLength.Value, useWithDotTruncate);

                stringProperty.SetValue(entity, newValue);
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
        /// <param name="truncateType">
        ///     (Optional) Type of the truncate.
        ///     Truncate string from the beginning or at the end.
        /// </param>
        /// <returns>Processed/parsed entity with new values</returns>
        /// <typeparam name="TEntity">Current entity type</typeparam>
        /// <remarks></remarks>
        public static TEntity ApplyStringMaxAllowedLength<TEntity>(
            TEntity entity,
            string propertyName,
            bool useDots = true,
            StringTruncateType truncateType = StringTruncateType.AtTheEndOf)
            where TEntity : class
        {
            if (entity.IsNull()) 
                throw new ArgumentNullException(nameof(entity));
            if (!propertyName.IsPresent()) 
                return entity;

            var entityType = typeof(TEntity);
            var stringProperty = EntityStringPropertyCache.GetStringProperty(entityType, propertyName);
            if (stringProperty.IsNotNull())
            {
                var currentValue = stringProperty.GetValue(entity);
                if (!currentValue.IsPresent()) return entity;

                var maxLength = stringProperty.MaxLength;
                if (maxLength.IsNull())
                    return entity;

                if (!(currentValue.Length > maxLength))
                    return entity;

                var newValue = truncateType == StringTruncateType.AtTheEndOf
                    ? currentValue.Truncate(maxLength.Value, useDots)
                    : currentValue.TruncateAtStart(maxLength.Value, useDots);

                stringProperty.SetValue(entity, newValue);
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
        public static TEntity ApplyStringMaxAllowedLength<TEntity>(
            TEntity entity,
            IReadOnlyCollection<PropertyOption> options,
            bool processOnlyAssigned = false)
            where TEntity : class
        {
            if (entity.IsNull()) 
                throw new ArgumentNullException(nameof(entity));
            if (options.IsNull()) 
                throw new ArgumentNullException(nameof(options));

            var entityType = typeof(TEntity);
            var propertiesOptions = options
                .Where(x => x.IsNotNull() && x.Name.IsPresent())
                .GroupBy(x => x.Name)
                .ToDictionary(x => x.Key, x => x.First(), StringComparer.Ordinal);

            var stringProperties = EntityStringPropertyCache.GetStringProperties(entityType);
            foreach (var stringProperty in stringProperties)
            {
                if (processOnlyAssigned && !propertiesOptions.ContainsKey(stringProperty.Name))
                    continue;

                var currentValue = stringProperty.GetValue(entity);
                if (!currentValue.IsPresent()) continue;

                var maxLength = stringProperty.MaxLength;

                if (maxLength.IsNull()) continue;
                if (!(currentValue.Length > maxLength)) continue;

                propertiesOptions.TryGetValue(stringProperty.Name, out var propOption);
                var useWithDotTruncate = propOption?.UseDots ?? false;
                var truncateType = propOption?.TruncateType ?? StringTruncateType.AtTheEndOf;

                var newValue = truncateType == StringTruncateType.AtTheEndOf
                    ? currentValue.Truncate(maxLength.Value, useWithDotTruncate)
                    : currentValue.TruncateAtStart(maxLength.Value, useWithDotTruncate);

                stringProperty.SetValue(entity, newValue);
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
        /// <param name="truncateType">
        ///     (Optional) Type of the truncate. 
        ///     Truncate string from the beginning or at the end.
        /// </param>
        /// <returns>Processed/parsed property with new value</returns>
        /// <typeparam name="TEntity">Current entity type</typeparam>
        /// <remarks></remarks>
        public static string ApplyPropStringMaxAllowedLength<TEntity>(
            TEntity entity,
            string propertyName,
            bool useDots = true,
            StringTruncateType truncateType = StringTruncateType.AtTheEndOf)
            where TEntity : class
        {
            if (entity.IsNull()) throw new ArgumentNullException(nameof(entity));
            if (!propertyName.IsPresent()) return null;

            var entityType = typeof(TEntity);
            var stringProperty = EntityStringPropertyCache.GetStringProperty(entityType, propertyName);
            if (stringProperty.IsNotNull())
            {
                var currentValue = stringProperty.GetValue(entity);
                if (!currentValue.IsPresent()) return currentValue;

                var maxLength = stringProperty.MaxLength;
                if (maxLength.IsNull()) return currentValue;
                if (!(currentValue.Length > maxLength)) return currentValue;

                var newValue = truncateType == StringTruncateType.AtTheEndOf
                    ? currentValue.Truncate(maxLength.Value, useDots)
                    : currentValue.TruncateAtStart(maxLength.Value, useDots);

                return newValue;
            }

            return null;
        }
    }
}