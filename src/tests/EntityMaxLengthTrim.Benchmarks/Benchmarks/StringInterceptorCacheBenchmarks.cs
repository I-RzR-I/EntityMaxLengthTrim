// ***********************************************************************
//  Assembly          : RzR.Shared.Entity.EntityMaxLengthTrim.Benchmarks
//  Author            : RzR
//  Created           : 17-05-2026 21:05
// 
//  Last Modified By : RzR
//  Last Modified On : 18-05-2026 22:00
//  ***********************************************************************
//  <copyright file="StringInterceptorCacheBenchmarks.cs" company="RzR SOFT & TECH">
//      Copyright (c) RzR. All rights reserved.
//  </copyright>
//  <contact>
//      https://iamrzr.dev/contact
//  </contact>
//  <summary></summary>
//  ***********************************************************************

#region U S I N G

using System.ComponentModel.DataAnnotations;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using RzR.Extensions.EntityLength.Attributes;
using RzR.Extensions.EntityLength.Enums;
using RzR.Extensions.EntityLength.Interceptors;
using RzR.Extensions.EntityLength.Options;

#endregion

namespace EntityMaxLengthTrim.Benchmarks.Benchmarks
{
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [SimpleJob(RuntimeMoniker.Net80, 1, 3, 10)]
    [InvocationCount(1)]
    public class StringInterceptorCacheBenchmarks
    {
        private const string DescriptionPropertyName = nameof(CacheBenchmarkEntity.Description);

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

        private readonly TrimOption _trimOption = new()
        {
            Policy = TrimPolicy.Silent,
            UseDots = false,
            TruncateType = StringTruncateType.AtTheEndOf,
            ApplyForceTrimEnd = false
        };

        private CacheBenchmarkEntity _withinLimitEntity;

        [GlobalSetup]
        public void Setup()
        {
            _withinLimitEntity = new CacheBenchmarkEntity
            {
                Name = BuildPayload('N', 16),
                FullName = BuildPayload('F', 32),
                Description = BuildPayload('D', 24),
                Alias = BuildPayload('A', 12),
                City = BuildPayload('C', 18),
                Country = BuildPayload('Y', 20),
                Notes = BuildPayload('O', 36),
                Tag = BuildPayload('T', 10),
                Version = 7,
                CreatedOn = new DateTime(2026, 5, 17, 0, 0, 0, DateTimeKind.Utc)
            };

            PrimeCaches();
        }

        [IterationSetup(Target = nameof(WarmCache_AllProperties))]
        public void SetupWarmAllProperties()
        {
            PrimeCaches();
        }

        [IterationSetup(Target = nameof(ColdCache_AllProperties))]
        public void SetupColdAllProperties()
        {
            ClearMetadataCaches();
        }

        [IterationSetup(Target = nameof(WarmCache_SinglePropertyLookup))]
        public void SetupWarmSingleProperty()
        {
            PrimeCaches();
        }

        [IterationSetup(Target = nameof(ColdCache_SinglePropertyLookup))]
        public void SetupColdSingleProperty()
        {
            ClearMetadataCaches();
        }

        [Benchmark(Baseline = true)]
        public CacheBenchmarkEntity WarmCache_AllProperties()
        {
            var entity = Clone(_withinLimitEntity);
            return StringInterceptor.ApplyStringMaxAllowedLength(entity, _trimOption);
        }

        [Benchmark]
        public CacheBenchmarkEntity ColdCache_AllProperties()
        {
            var entity = Clone(_withinLimitEntity);
            return StringInterceptor.ApplyStringMaxAllowedLength(entity, _trimOption);
        }

        [Benchmark]
        public string WarmCache_SinglePropertyLookup()
        {
            return StringInterceptor.ApplyPropStringMaxAllowedLength(
                Clone(_withinLimitEntity),
                DescriptionPropertyName,
                false,
                StringTruncateType.AtTheEndOf,
                false);
        }

        [Benchmark]
        public string ColdCache_SinglePropertyLookup()
        {
            return StringInterceptor.ApplyPropStringMaxAllowedLength(
                Clone(_withinLimitEntity),
                DescriptionPropertyName,
                false,
                StringTruncateType.AtTheEndOf,
                false);
        }

        private void PrimeCaches()
        {
            ClearMetadataCaches();

            StringInterceptor.ApplyStringMaxAllowedLength(Clone(_withinLimitEntity), _trimOption);
            StringInterceptor.ApplyPropStringMaxAllowedLength(
                Clone(_withinLimitEntity),
                DescriptionPropertyName,
                false,
                StringTruncateType.AtTheEndOf,
                false);
        }

        private static void ClearMetadataCaches()
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

        private static CacheBenchmarkEntity Clone(CacheBenchmarkEntity source)
        {
            return new CacheBenchmarkEntity
            {
                Name = source.Name,
                FullName = source.FullName,
                Description = source.Description,
                Alias = source.Alias,
                City = source.City,
                Country = source.Country,
                Notes = source.Notes,
                Tag = source.Tag,
                Version = source.Version,
                CreatedOn = source.CreatedOn
            };
        }

        private static string BuildPayload(char fill, int length)
        {
            return new string(fill, length);
        }

        public class CacheBenchmarkEntity
        {
            [MaxLength(32)] public string Name { get; set; }

            [StringLength(64)] public string FullName { get; set; }

            [MaxAllowedLength(48)] public string Description { get; set; }

            [MaxLength(24)] public string Alias { get; set; }

            [StringLength(40)] public string City { get; set; }

            [MaxAllowedLength(56)] public string Country { get; set; }

            [MaxLength(72)] public string Notes { get; set; }

            [StringLength(16)] public string Tag { get; set; }

            public int Version { get; set; }

            public DateTime CreatedOn { get; set; }
        }
    }
}