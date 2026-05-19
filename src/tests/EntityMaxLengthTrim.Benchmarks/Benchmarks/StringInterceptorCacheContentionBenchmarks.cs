// ***********************************************************************
//  Assembly          : RzR.Shared.Entity.EntityMaxLengthTrim.Benchmarks
//  Author            : RzR
//  Created           : 17-05-2026 21:05
// 
//  Last Modified By : RzR
//  Last Modified On : 18-05-2026 22:00
//  ***********************************************************************
//  <copyright file="StringInterceptorCacheContentionBenchmarks.cs" company="RzR SOFT & TECH">
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
    public class StringInterceptorCacheContentionBenchmarks
    {
        private readonly TrimOption _trimOption = new()
        {
            Policy = TrimPolicy.Silent,
            UseDots = false,
            TruncateType = StringTruncateType.AtTheEndOf,
            ApplyForceTrimEnd = false
        };

        private ContentionBenchmarkEntity _withinLimitEntity;

        [Params(1, 4, 8)] public int WorkerCount { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            _withinLimitEntity = new ContentionBenchmarkEntity
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
                ExternalId = BuildPayload('X', 18)
            };

            PrimeCache();
        }

        [IterationSetup(Target = nameof(WarmCache_ConcurrentAccess))]
        public void SetupWarmCacheAccess()
        {
            PrimeCache();
        }

        [IterationSetup(Target = nameof(ColdCache_FirstHitConcurrentAccess))]
        public void SetupColdCacheAccess()
        {
            EntityStringPropertyCacheBenchmarkHarness.ClearMetadataCaches();
        }

        [Benchmark(Baseline = true)]
        public int WarmCache_ConcurrentAccess()
        {
            return RunConcurrentAccess();
        }

        [Benchmark]
        public int ColdCache_FirstHitConcurrentAccess()
        {
            return RunConcurrentAccess();
        }

        private void PrimeCache()
        {
            EntityStringPropertyCacheBenchmarkHarness.ClearMetadataCaches();
            StringInterceptor.ApplyStringMaxAllowedLength(Clone(_withinLimitEntity), _trimOption);
        }

        private int RunConcurrentAccess()
        {
            var totalLength = 0;
            var options = new ParallelOptions { MaxDegreeOfParallelism = WorkerCount };

            Parallel.For(0, WorkerCount, options, () => 0, (index, _, localSum) =>
            {
                var result = StringInterceptor.ApplyStringMaxAllowedLength(Clone(_withinLimitEntity), _trimOption);

                return localSum
                       + result.Name.Length
                       + result.FullName.Length
                       + result.Description.Length
                       + result.Alias.Length
                       + result.City.Length
                       + result.Country.Length
                       + result.Notes.Length
                       + result.Tag.Length
                       + result.Region.Length
                       + result.ExternalId.Length;
            }, localSum => Interlocked.Add(ref totalLength, localSum));

            return totalLength;
        }

        private static ContentionBenchmarkEntity Clone(ContentionBenchmarkEntity source)
        {
            return new ContentionBenchmarkEntity
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
                ExternalId = source.ExternalId
            };
        }

        private static string BuildPayload(char fill, int length)
        {
            return new string(fill, length);
        }

        public class ContentionBenchmarkEntity
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

            [StringLength(32)] public string ExternalId { get; set; }
        }
    }
}