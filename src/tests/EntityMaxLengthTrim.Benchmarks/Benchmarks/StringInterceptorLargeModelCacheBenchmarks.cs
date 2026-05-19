// ***********************************************************************
//  Assembly          : RzR.Shared.Entity.EntityMaxLengthTrim.Benchmarks
//  Author            : RzR
//  Created           : 17-05-2026 21:05
// 
//  Last Modified By : RzR
//  Last Modified On : 18-05-2026 22:00
//  ***********************************************************************
//  <copyright file="StringInterceptorLargeModelCacheBenchmarks.cs" company="RzR SOFT & TECH">
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
    public class StringInterceptorLargeModelCacheBenchmarks
    {
        private const string LookupPropertyName = nameof(LargeCacheBenchmarkEntity.ExternalId);

        private readonly TrimOption _trimOption = new()
        {
            Policy = TrimPolicy.Silent,
            UseDots = false,
            TruncateType = StringTruncateType.AtTheEndOf,
            ApplyForceTrimEnd = false
        };

        private LargeCacheBenchmarkEntity _withinLimitEntity;

        [GlobalSetup]
        public void Setup()
        {
            _withinLimitEntity = new LargeCacheBenchmarkEntity
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
                Version = 11,
                CreatedOn = new DateTime(2026, 5, 17, 0, 0, 0, DateTimeKind.Utc),
                IsActive = true
            };

            PrimeCaches();
        }

        [IterationSetup(Target = nameof(WarmCache_AllProperties_LargeModel))]
        public void SetupWarmAllProperties()
        {
            PrimeCaches();
        }

        [IterationSetup(Target = nameof(ColdCache_AllProperties_LargeModel))]
        public void SetupColdAllProperties()
        {
            EntityStringPropertyCacheBenchmarkHarness.ClearMetadataCaches();
        }

        [IterationSetup(Target = nameof(WarmCache_SinglePropertyLookup_LargeModel))]
        public void SetupWarmSingleProperty()
        {
            PrimeCaches();
        }

        [IterationSetup(Target = nameof(ColdCache_SinglePropertyLookup_LargeModel))]
        public void SetupColdSingleProperty()
        {
            EntityStringPropertyCacheBenchmarkHarness.ClearMetadataCaches();
        }

        [Benchmark(Baseline = true)]
        public LargeCacheBenchmarkEntity WarmCache_AllProperties_LargeModel()
        {
            var entity = Clone(_withinLimitEntity);
            return StringInterceptor.ApplyStringMaxAllowedLength(entity, _trimOption);
        }

        [Benchmark]
        public LargeCacheBenchmarkEntity ColdCache_AllProperties_LargeModel()
        {
            var entity = Clone(_withinLimitEntity);
            return StringInterceptor.ApplyStringMaxAllowedLength(entity, _trimOption);
        }

        [Benchmark]
        public string WarmCache_SinglePropertyLookup_LargeModel()
        {
            return StringInterceptor.ApplyPropStringMaxAllowedLength(
                Clone(_withinLimitEntity),
                LookupPropertyName,
                false,
                StringTruncateType.AtTheEndOf,
                false);
        }

        [Benchmark]
        public string ColdCache_SinglePropertyLookup_LargeModel()
        {
            return StringInterceptor.ApplyPropStringMaxAllowedLength(
                Clone(_withinLimitEntity),
                LookupPropertyName,
                false,
                StringTruncateType.AtTheEndOf,
                false);
        }

        private void PrimeCaches()
        {
            EntityStringPropertyCacheBenchmarkHarness.ClearMetadataCaches();

            StringInterceptor.ApplyStringMaxAllowedLength(Clone(_withinLimitEntity), _trimOption);
            StringInterceptor.ApplyPropStringMaxAllowedLength(
                Clone(_withinLimitEntity),
                LookupPropertyName,
                false,
                StringTruncateType.AtTheEndOf,
                false);
        }

        private static LargeCacheBenchmarkEntity Clone(LargeCacheBenchmarkEntity source)
        {
            return new LargeCacheBenchmarkEntity
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

        public class LargeCacheBenchmarkEntity
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