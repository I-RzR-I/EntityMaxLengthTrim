// ***********************************************************************
//  Assembly          : RzR.Shared.Entity.EntityMaxLengthTrim.Benchmarks
//  Author            : RzR
//  Created           : 17-05-2026 21:05
// 
//  Last Modified By : RzR
//  Last Modified On : 18-05-2026 22:00
//  ***********************************************************************
//  <copyright file="EntityStringPropertyCacheBenchmarkHarness.cs" company="RzR SOFT & TECH">
//      Copyright (c) RzR. All rights reserved.
//  </copyright>
//  <contact>
//      https://iamrzr.dev/contact
//  </contact>
//  <summary></summary>
//  ***********************************************************************

#region U S I N G

using System.Reflection;
using RzR.Extensions.EntityLength.Interceptors;

#endregion

namespace EntityMaxLengthTrim.Benchmarks.Benchmarks
{
    internal static class EntityStringPropertyCacheBenchmarkHarness
    {
        private static readonly Type CacheType = typeof(StringInterceptor).Assembly.GetType(
                                                     "RzR.Extensions.EntityLength.Interceptors.Internal.EntityStringPropertyCache",
                                                     true)
                                                 ?? throw new InvalidOperationException(
                                                     "Unable to locate the entity metadata cache type.");

        private static readonly FieldInfo EntityPropertiesField = GetRequiredField("EntityProperties");
        private static readonly FieldInfo EntityPropertyByNameField = GetRequiredField("EntityPropertyByName");
        private static readonly MethodInfo EntityPropertiesClearMethod = GetRequiredClearMethod(EntityPropertiesField);

        private static readonly MethodInfo EntityPropertyByNameClearMethod =
            GetRequiredClearMethod(EntityPropertyByNameField);

        internal static void ClearMetadataCaches()
        {
            EntityPropertiesClearMethod.Invoke(EntityPropertiesField.GetValue(null), null);
            EntityPropertyByNameClearMethod.Invoke(EntityPropertyByNameField.GetValue(null), null);
        }

        private static FieldInfo GetRequiredField(string fieldName)
        {
            return CacheType.GetField(fieldName, BindingFlags.Static | BindingFlags.NonPublic)
                   ?? throw new MissingFieldException(CacheType.FullName, fieldName);
        }

        private static MethodInfo GetRequiredClearMethod(FieldInfo fieldInfo)
        {
            return fieldInfo.FieldType.GetMethod("Clear", BindingFlags.Instance | BindingFlags.Public)
                   ?? throw new MissingMethodException(fieldInfo.FieldType.FullName, "Clear");
        }
    }
}