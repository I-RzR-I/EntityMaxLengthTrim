// ***********************************************************************
//  Assembly          : RzR.Shared.Entity.EntityMaxLengthTrim.Benchmarks
//  Author            : RzR
//  Created           : 17-05-2026 21:05
// 
//  Last Modified By : RzR
//  Last Modified On : 18-05-2026 22:00
//  ***********************************************************************
//  <copyright file="StringInterceptorPropertyOptionBenchmarks.cs" company="RzR SOFT & TECH">
//      Copyright (c) RzR. All rights reserved.
//  </copyright>
//  <contact>
//      https://iamrzr.dev/contact
//  </contact>
//  <summary></summary>
//  ***********************************************************************

#region U S I N G

using System.ComponentModel.DataAnnotations;
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
    public class StringInterceptorPropertyOptionBenchmarks
    {
        private static readonly string[] AllConfiguredPropertyNames =
        {
            nameof(PropertyOptionsBenchmarkEntity.Name),
            nameof(PropertyOptionsBenchmarkEntity.FullName),
            nameof(PropertyOptionsBenchmarkEntity.Description),
            nameof(PropertyOptionsBenchmarkEntity.Alias),
            nameof(PropertyOptionsBenchmarkEntity.City),
            nameof(PropertyOptionsBenchmarkEntity.Country),
            nameof(PropertyOptionsBenchmarkEntity.Notes),
            nameof(PropertyOptionsBenchmarkEntity.Tag),
            nameof(PropertyOptionsBenchmarkEntity.Region),
            nameof(PropertyOptionsBenchmarkEntity.District),
            nameof(PropertyOptionsBenchmarkEntity.AddressLine1),
            nameof(PropertyOptionsBenchmarkEntity.AddressLine2),
            nameof(PropertyOptionsBenchmarkEntity.PostalCode),
            nameof(PropertyOptionsBenchmarkEntity.Category),
            nameof(PropertyOptionsBenchmarkEntity.SubCategory),
            nameof(PropertyOptionsBenchmarkEntity.Owner),
            nameof(PropertyOptionsBenchmarkEntity.ReferenceCode),
            nameof(PropertyOptionsBenchmarkEntity.Metadata),
            nameof(PropertyOptionsBenchmarkEntity.Summary),
            nameof(PropertyOptionsBenchmarkEntity.Detail),
            nameof(PropertyOptionsBenchmarkEntity.Label),
            nameof(PropertyOptionsBenchmarkEntity.Title),
            nameof(PropertyOptionsBenchmarkEntity.Segment),
            nameof(PropertyOptionsBenchmarkEntity.ExternalId)
        };

        private static readonly string[] PartiallyAssignedPropertyNames =
        {
            nameof(PropertyOptionsBenchmarkEntity.Name),
            nameof(PropertyOptionsBenchmarkEntity.FullName),
            nameof(PropertyOptionsBenchmarkEntity.Description),
            nameof(PropertyOptionsBenchmarkEntity.AddressLine1),
            nameof(PropertyOptionsBenchmarkEntity.Metadata),
            nameof(PropertyOptionsBenchmarkEntity.ExternalId)
        };

        private readonly TrimOption _trimOption = new()
        {
            Policy = TrimPolicy.Silent,
            UseDots = false,
            TruncateType = StringTruncateType.AtTheEndOf,
            ApplyForceTrimEnd = false
        };

        private PropertyOption[] _heavyOptionsAllAssigned;
        private PropertyOption[] _heavyOptionsPartialAssigned;

        private PropertyOptionsBenchmarkEntity _withinLimitEntity;

        [Params(32, 128, 512, 2048)] public int ConfiguredEntryCount { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            _withinLimitEntity = new PropertyOptionsBenchmarkEntity
            {
                Name = BuildPayload('N', 16),
                FullName = BuildPayload('F', 32),
                Description = BuildPayload('D', 24),
                Alias = BuildPayload('A', 12),
                City = BuildPayload('C', 18),
                Country = BuildPayload('Y', 20),
                Notes = BuildPayload('O', 36),
                Tag = BuildPayload('T', 10),
                Region = BuildPayload('R', 14),
                District = BuildPayload('I', 15),
                AddressLine1 = BuildPayload('L', 40),
                AddressLine2 = BuildPayload('M', 36),
                PostalCode = BuildPayload('P', 12),
                Category = BuildPayload('G', 14),
                SubCategory = BuildPayload('S', 16),
                Owner = BuildPayload('W', 26),
                ReferenceCode = BuildPayload('E', 18),
                Metadata = BuildPayload('H', 48),
                Summary = BuildPayload('U', 60),
                Detail = BuildPayload('J', 64),
                Label = BuildPayload('B', 16),
                Title = BuildPayload('Q', 22),
                Segment = BuildPayload('X', 18),
                ExternalId = BuildPayload('Z', 20),
                Version = 13,
                CreatedOn = new DateTime(2026, 5, 17, 0, 0, 0, DateTimeKind.Utc),
                IsActive = true
            };

            _heavyOptionsAllAssigned = BuildOptions(AllConfiguredPropertyNames, ConfiguredEntryCount);
            _heavyOptionsPartialAssigned = BuildOptions(PartiallyAssignedPropertyNames, ConfiguredEntryCount);

            PrimeMetadataCache();
        }

        [IterationSetup]
        public void IterationSetup()
        {
            PrimeMetadataCache();
        }

        [Benchmark(Baseline = true)]
        public PropertyOptionsBenchmarkEntity TrimOption_WithinLimits_Baseline()
        {
            var entity = Clone(_withinLimitEntity);
            return StringInterceptor.ApplyStringMaxAllowedLength(entity, _trimOption);
        }

        [Benchmark]
        public PropertyOptionsBenchmarkEntity PropertyOptions_ManyConfiguredEntries_ProcessAll()
        {
            var entity = Clone(_withinLimitEntity);
            return StringInterceptor.ApplyStringMaxAllowedLength(entity, _heavyOptionsAllAssigned, false, false);
        }

        [Benchmark]
        public PropertyOptionsBenchmarkEntity PropertyOptions_ManyConfiguredEntries_ProcessOnlyAssigned()
        {
            var entity = Clone(_withinLimitEntity);
            return StringInterceptor.ApplyStringMaxAllowedLength(entity, _heavyOptionsPartialAssigned, true, false);
        }

        private void PrimeMetadataCache()
        {
            EntityStringPropertyCacheBenchmarkHarness.ClearMetadataCaches();
            StringInterceptor.ApplyStringMaxAllowedLength(Clone(_withinLimitEntity), _trimOption);
        }

        private static PropertyOption[] BuildOptions(IReadOnlyList<string> actualPropertyNames,
            int configuredEntryCount)
        {
            var totalCount = Math.Max(configuredEntryCount, actualPropertyNames.Count);
            var options = new List<PropertyOption>(totalCount);

            for (var index = 0; index < actualPropertyNames.Count; index++)
                options.Add(new PropertyOption
                {
                    Name = actualPropertyNames[index],
                    UseDots = index % 2 == 0,
                    TruncateType = index % 3 == 0
                        ? StringTruncateType.AtTheStartOf
                        : StringTruncateType.AtTheEndOf,
                    ApplyForceTrimEnd = index % 2 != 0
                });

            for (var index = options.Count; index < totalCount; index++)
                options.Add(new PropertyOption
                {
                    Name = $"Noise_{index:D4}",
                    UseDots = index % 2 == 0,
                    TruncateType = index % 5 == 0
                        ? StringTruncateType.AtTheStartOf
                        : StringTruncateType.AtTheEndOf,
                    ApplyForceTrimEnd = index % 2 == 0
                });

            return options.ToArray();
        }

        private static PropertyOptionsBenchmarkEntity Clone(PropertyOptionsBenchmarkEntity source)
        {
            return new PropertyOptionsBenchmarkEntity
            {
                Name = source.Name,
                FullName = source.FullName,
                Description = source.Description,
                Alias = source.Alias,
                City = source.City,
                Country = source.Country,
                Notes = source.Notes,
                Tag = source.Tag,
                Region = source.Region,
                District = source.District,
                AddressLine1 = source.AddressLine1,
                AddressLine2 = source.AddressLine2,
                PostalCode = source.PostalCode,
                Category = source.Category,
                SubCategory = source.SubCategory,
                Owner = source.Owner,
                ReferenceCode = source.ReferenceCode,
                Metadata = source.Metadata,
                Summary = source.Summary,
                Detail = source.Detail,
                Label = source.Label,
                Title = source.Title,
                Segment = source.Segment,
                ExternalId = source.ExternalId,
                Version = source.Version,
                CreatedOn = source.CreatedOn,
                IsActive = source.IsActive
            };
        }

        private static string BuildPayload(char fill, int length)
        {
            return new string(fill, length);
        }

        public class PropertyOptionsBenchmarkEntity
        {
            [MaxLength(32)] public string Name { get; set; }

            [StringLength(64)] public string FullName { get; set; }

            [MaxAllowedLength(48)] public string Description { get; set; }

            [MaxLength(24)] public string Alias { get; set; }

            [StringLength(40)] public string City { get; set; }

            [MaxAllowedLength(56)] public string Country { get; set; }

            [MaxLength(72)] public string Notes { get; set; }

            [StringLength(16)] public string Tag { get; set; }

            [MaxAllowedLength(28)] public string Region { get; set; }

            [MaxLength(30)] public string District { get; set; }

            [StringLength(80)] public string AddressLine1 { get; set; }

            [MaxAllowedLength(80)] public string AddressLine2 { get; set; }

            [MaxLength(20)] public string PostalCode { get; set; }

            [StringLength(24)] public string Category { get; set; }

            [MaxAllowedLength(24)] public string SubCategory { get; set; }

            [MaxLength(50)] public string Owner { get; set; }

            [StringLength(36)] public string ReferenceCode { get; set; }

            [MaxAllowedLength(96)] public string Metadata { get; set; }

            [MaxLength(120)] public string Summary { get; set; }

            [StringLength(128)] public string Detail { get; set; }

            [MaxAllowedLength(32)] public string Label { get; set; }

            [MaxLength(44)] public string Title { get; set; }

            [StringLength(40)] public string Segment { get; set; }

            [MaxAllowedLength(48)] public string ExternalId { get; set; }

            public int Version { get; set; }

            public DateTime CreatedOn { get; set; }

            public bool IsActive { get; set; }
        }
    }
}