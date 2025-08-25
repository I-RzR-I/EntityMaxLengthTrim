// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.EntityMaxLengthTrim
//  Author           : RzR
//  Created On       : 2023-10-06 22:31
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-10-07 00:54
// ***********************************************************************
//  <copyright file="FluentExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Collections.Generic;
using EntityMaxLengthTrim.Enums;
using EntityMaxLengthTrim.Interceptors;
using EntityMaxLengthTrim.Options;

#endregion

namespace EntityMaxLengthTrim.Extensions
{
    /// <summary>
    ///     Fluent extensions for safe store object
    /// </summary>
    public static class FluentExtensions
    {
        /// <summary>
        ///     Prepare initialized object to save store string properties.
        /// </summary>
        /// <param name="initSourceObject">Required. Initialized object with data.</param>
        /// <param name="useDotOnEnd">
        ///     Optional. The default value is false. If set to <see langword="true" />, then ant the end of
        ///     string prop will be '...'; otherwise, truncate to max length.
        /// </param>
        /// <param name="truncateType">
        ///     (Optional) Type of the truncate.
        ///     Truncate string from the beginning or at the end.
        /// </param>
        /// <returns></returns>
        /// <typeparam name="TEntity">Type of initialized object.</typeparam>
        /// <remarks></remarks>
        public static TEntity ToSafeStoreStrings<TEntity>(
            this TEntity initSourceObject,
            bool useDotOnEnd = false,
            StringTruncateType truncateType = StringTruncateType.AtTheEndOf)
            where TEntity : class
            => StringInterceptor.ApplyStringMaxAllowedLength(initSourceObject, useDotOnEnd, truncateType);

        /// <summary>
        ///     Prepare initialized object to save store string properties.
        /// </summary>
        /// <param name="initSourceObject">Required. Initialized object with data.</param>
        /// <param name="truncateWithDots">
        ///     Required. The default value is false. If set to <see langword="true" />, then ant the end of
        ///     string prop will be '...'; otherwise, truncate to max length.
        /// </param>
        /// <param name="processOnlyAssigned">
        ///     Optional. The default value is false.If set to <see langword="true" />, then
        ///     process only specified props; otherwise, process all props.
        /// </param>
        /// <param name="truncateType">
        ///     (Optional) Type of the truncate.
        ///     Truncate string from the beginning or at the end.
        /// </param>
        /// <returns></returns>
        /// <typeparam name="TEntity">Type of initialized object.</typeparam>
        /// <remarks></remarks>
        public static TEntity ToSafeStoreStrings<TEntity>(
            this TEntity initSourceObject,
            IReadOnlyCollection<string> truncateWithDots,
            bool processOnlyAssigned = false,
            StringTruncateType truncateType = StringTruncateType.AtTheEndOf)
            where TEntity : class
            => StringInterceptor.ApplyStringMaxAllowedLength(initSourceObject, truncateWithDots, processOnlyAssigned, truncateType);

        /// <summary>
        ///     Prepare initialized object to save store string properties.
        /// </summary>
        /// <param name="initSourceObject">Required. Initialized object with data.</param>
        /// <param name="options">Required. Properties options</param>
        /// <param name="processOnlyAssigned">
        ///     Optional. The default value is false.If set to <see langword="true" />, then
        ///     process only specified props; otherwise, process all props.
        /// </param>
        /// <returns></returns>
        /// <typeparam name="TEntity">Type of initialized object.</typeparam>
        /// <remarks></remarks>
        public static TEntity ToSafeStoreStrings<TEntity>(
            this TEntity initSourceObject,
            IReadOnlyCollection<PropertyOption> options,
            bool processOnlyAssigned = false)
            where TEntity : class
            => StringInterceptor.ApplyStringMaxAllowedLength(initSourceObject, options, processOnlyAssigned);
    }
}