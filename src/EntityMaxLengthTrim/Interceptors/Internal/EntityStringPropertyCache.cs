// ***********************************************************************
//  Assembly         : RzR.Extensions.EntityLength
//  Author           : RzR
//  Created On       : 2026-04-21 14:04
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-04-21 14:42
// ***********************************************************************
//  <copyright file="EntityStringPropertyCache.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using RzR.Extensions.EntityLength.Extensions.Internal;

#endregion

namespace RzR.Extensions.EntityLength.Interceptors.Internal
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Cached metadata for string property processing.
    /// </summary>
    /// =================================================================================================
    internal sealed class EntityStringPropertyMeta
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the getter.
        /// </summary>
        /// =================================================================================================
        private readonly Func<object, string> _getter;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the setter.
        /// </summary>
        /// =================================================================================================
        private readonly Action<object, string> _setter;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="EntityStringPropertyMeta" /> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="propertyInfo">Property information.</param>
        /// <param name="maxLength">The maximum allowed length.</param>
        /// =================================================================================================
        internal EntityStringPropertyMeta(PropertyInfo propertyInfo, int? maxLength)
        {
            PropertyInfo = propertyInfo ?? throw new ArgumentNullException(nameof(propertyInfo));
            MaxLength = NormalizeMaxLength(maxLength);

            _getter = BuildGetter(propertyInfo);
            _setter = BuildSetter(propertyInfo);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the property name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        /// =================================================================================================
        internal string Name => PropertyInfo.Name;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets property info.
        /// </summary>
        /// <value>
        ///     Information describing the property.
        /// </value>
        /// =================================================================================================
        internal PropertyInfo PropertyInfo { get; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the maximum allowed string length for this property.
        /// </summary>
        /// <value>
        ///     The maximum length of the property.
        /// </value>
        /// =================================================================================================
        internal int? MaxLength { get; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets string value from entity.
        /// </summary>
        /// <param name="entity">Target entity.</param>
        /// <returns>
        ///     Property value.
        /// </returns>
        /// =================================================================================================
        internal string GetValue(object entity)
        {
            return _getter(entity);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Sets string value on entity.
        /// </summary>
        /// <param name="entity">Target entity.</param>
        /// <param name="value">New property value.</param>
        /// =================================================================================================
        internal void SetValue(object entity, string value)
        {
            _setter(entity, value);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Normalize maximum length.
        /// </summary>
        /// <param name="value">New property value.</param>
        /// <returns>
        ///     An int?
        /// </returns>
        /// =================================================================================================
        private static int? NormalizeMaxLength(int? value)
        {
            return value.HasValue && value.Value > 0 ? value : null;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Builds a getter.
        /// </summary>
        /// <param name="propertyInfo">Property information.</param>
        /// <returns>
        ///     A function delegate that yields a string.
        /// </returns>
        /// =================================================================================================
        private static Func<object, string> BuildGetter(PropertyInfo propertyInfo)
        {
            var instance = Expression.Parameter(typeof(object), "i");
            var cast = Expression.Convert(instance, propertyInfo.DeclaringType!);
            var access = Expression.Property(cast, propertyInfo);

            return Expression.Lambda<Func<object, string>>(access, instance).Compile();
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Builds a setter.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when the requested operation is invalid.
        /// </exception>
        /// <param name="propertyInfo">Property information.</param>
        /// <returns>
        ///     An Action&lt;object,string&gt;
        /// </returns>
        /// =================================================================================================
        private static Action<object, string> BuildSetter(PropertyInfo propertyInfo)
        {
            var setMethod = propertyInfo.SetMethod;
            if (setMethod == null)
                return (_, __) =>
                    throw new InvalidOperationException(
                        $"Property '{propertyInfo.DeclaringType?.Name}.{propertyInfo.Name}' has no accessible setter.");

            var instance = Expression.Parameter(typeof(object), "i");
            var value = Expression.Parameter(typeof(string), "v");
            var cast = Expression.Convert(instance, propertyInfo.DeclaringType!);
            var call = Expression.Call(cast, setMethod, value);

            return Expression.Lambda<Action<object, string>>(call, instance, value).Compile();
        }
    }

    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Cache for entity string properties and resolved max-length metadata.
    /// </summary>
    /// =================================================================================================
    internal static class EntityStringPropertyCache
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the entity properties.
        /// </summary>
        /// =================================================================================================
        private static readonly ConcurrentDictionary<Type, EntityStringPropertyMeta[]> EntityProperties =
            new ConcurrentDictionary<Type, EntityStringPropertyMeta[]>();

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) name of the entity property by.
        /// </summary>
        /// =================================================================================================
        private static readonly ConcurrentDictionary<Type, IReadOnlyDictionary<string, EntityStringPropertyMeta>>
            EntityPropertyByName =
                new ConcurrentDictionary<Type, IReadOnlyDictionary<string, EntityStringPropertyMeta>>();

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Get all cached string properties for a type.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="entityType">Entity type.</param>
        /// <returns>
        ///     Cached list of metadata entries.
        /// </returns>
        /// =================================================================================================
        internal static IReadOnlyList<EntityStringPropertyMeta> GetStringProperties(Type entityType)
        {
            if (entityType.IsNull())
                throw new ArgumentNullException(nameof(entityType));

            return EntityProperties.GetOrAdd(entityType, BuildProperties);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Get cached string property metadata by property name.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="entityType">Entity type.</param>
        /// <param name="propertyName">Property name.</param>
        /// <returns>
        ///     Cached metadata entry or null.
        /// </returns>
        /// =================================================================================================
        internal static EntityStringPropertyMeta GetStringProperty(Type entityType, string propertyName)
        {
            if (entityType.IsNull())
                throw new ArgumentNullException(nameof(entityType));
            if (!propertyName.IsPresent())
                return null;

            var propertyMap = EntityPropertyByName.GetOrAdd(entityType, BuildPropertyMap);

            return propertyMap.TryGetValue(propertyName, out var propertyMeta)
                ? propertyMeta
                : null;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Builds the properties.
        /// </summary>
        /// <param name="entityType">Entity type.</param>
        /// <returns>
        ///     An EntityStringPropertyMeta[].
        /// </returns>
        /// =================================================================================================
        private static EntityStringPropertyMeta[] BuildProperties(Type entityType)
        {
            var properties = entityType.GetStringPropertyInfos()?
                .Where(x => x.CanRead && x.CanWrite && x.GetIndexParameters().Length == 0)
                .ToList();
            if (properties.IsNull() || properties?.Count == 0)
                return new EntityStringPropertyMeta[0];

            return properties?
                .Select(x => new EntityStringPropertyMeta(x, x.GetMaxAllowedLength()))
                .ToArray();
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Builds property map.
        /// </summary>
        /// <param name="entityType">Entity type.</param>
        /// <returns>
        ///     An IReadOnlyDictionary&lt;string,EntityStringPropertyMeta&gt;
        /// </returns>
        /// =================================================================================================
        private static IReadOnlyDictionary<string, EntityStringPropertyMeta> BuildPropertyMap(Type entityType)
        {
            var stringProperties = GetStringProperties(entityType);

            return stringProperties
                .ToDictionary(x => x.Name, x => x, StringComparer.Ordinal);
        }
    }
}