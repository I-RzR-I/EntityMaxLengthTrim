// ***********************************************************************
//  Assembly          : RzR.Shared.Entity.EntityMaxLengthTrim.Benchmarks
//  Author            : RzR
//  Created           : 17-05-2026 21:05
// 
//  Last Modified By : RzR
//  Last Modified On : 18-05-2026 22:00
//  ***********************************************************************
//  <copyright file="StringInterceptorBenchmarks.cs" company="RzR SOFT & TECH">
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
    public class StringInterceptorBenchmarks
    {
        private readonly PropertyOption[] _propertyOptions =
        {
            new()
            {
                Name = nameof(BenchmarkEntity.Name),
                UseDots = false,
                TruncateType = StringTruncateType.AtTheEndOf,
                ApplyForceTrimEnd = false
            },
            new()
            {
                Name = nameof(BenchmarkEntity.FullName),
                UseDots = true,
                TruncateType = StringTruncateType.AtTheEndOf,
                ApplyForceTrimEnd = true
            },
            new()
            {
                Name = nameof(BenchmarkEntity.Description),
                UseDots = false,
                TruncateType = StringTruncateType.AtTheStartOf,
                ApplyForceTrimEnd = false
            }
        };

        private readonly TrimOption _trimAtEndWithDots = new()
        {
            Policy = TrimPolicy.Silent,
            UseDots = true,
            TruncateType = StringTruncateType.AtTheEndOf,
            ApplyForceTrimEnd = true
        };

        private readonly TrimOption _trimAtEndWithoutDots = new()
        {
            Policy = TrimPolicy.Silent,
            UseDots = false,
            TruncateType = StringTruncateType.AtTheEndOf,
            ApplyForceTrimEnd = false
        };

        private readonly TrimOption _trimAtStartWithoutDots = new()
        {
            Policy = TrimPolicy.Silent,
            UseDots = false,
            TruncateType = StringTruncateType.AtTheStartOf,
            ApplyForceTrimEnd = false
        };

        private string _descriptionValue;
        private string _fullNameValue;

        private string _nameValue;
        private string _shortDescriptionValue;
        private string _shortFullNameValue;
        private string _shortNameValue;

        [Params(96, 256, 1024)] public int SourceLength { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            _nameValue = BuildPayload('N', SourceLength);
            _fullNameValue = BuildPayload('F', SourceLength, true);
            _descriptionValue = BuildPayload('D', SourceLength, true);
            _shortNameValue = BuildPayload('n', 16);
            _shortFullNameValue = BuildPayload('f', 24);
            _shortDescriptionValue = BuildPayload('d', 24);
        }

        [Benchmark(Baseline = true)]
        public BenchmarkEntity LegacyOverload_EndWithoutDots()
        {
            var entity = CreateOversizedEntity();
            return StringInterceptor.ApplyStringMaxAllowedLength(entity);
        }

        [Benchmark]
        public BenchmarkEntity TrimOption_EndWithDots_TrimTrailingSpaces()
        {
            var entity = CreateOversizedEntity();
            return StringInterceptor.ApplyStringMaxAllowedLength(entity, _trimAtEndWithDots);
        }

        [Benchmark]
        public BenchmarkEntity TrimOption_StartWithoutDots()
        {
            var entity = CreateOversizedEntity();
            return StringInterceptor.ApplyStringMaxAllowedLength(entity, _trimAtStartWithoutDots);
        }

        [Benchmark]
        public BenchmarkEntity PropertyOptions_PerPropertyOverrides()
        {
            var entity = CreateOversizedEntity();
            return StringInterceptor.ApplyStringMaxAllowedLength(entity, _propertyOptions, false, false);
        }

        [Benchmark]
        public string SingleProperty_EndWithDots()
        {
            return StringInterceptor.ApplyPropStringMaxAllowedLength(
                CreateOversizedEntity(),
                nameof(BenchmarkEntity.FullName),
                true,
                StringTruncateType.AtTheEndOf,
                true);
        }

        [Benchmark]
        public BenchmarkEntity NoOp_WithinLimits()
        {
            var entity = CreateWithinLimitEntity();
            return StringInterceptor.ApplyStringMaxAllowedLength(entity, _trimAtEndWithoutDots);
        }

        private BenchmarkEntity CreateOversizedEntity()
        {
            return new BenchmarkEntity
            {
                Name = _nameValue,
                FullName = _fullNameValue,
                Description = _descriptionValue
            };
        }

        private BenchmarkEntity CreateWithinLimitEntity()
        {
            return new BenchmarkEntity
            {
                Name = _shortNameValue,
                FullName = _shortFullNameValue,
                Description = _shortDescriptionValue
            };
        }

        private static string BuildPayload(char fill, int length, bool addTrailingSpaces = false)
        {
            if (!addTrailingSpaces || length < 4)
                return new string(fill, length);

            return new string(fill, length - 3) + "   ";
        }

        public class BenchmarkEntity
        {
            [MaxLength(32)] public string Name { get; set; }

            [StringLength(64)] public string FullName { get; set; }

            [MaxAllowedLength(48)] public string Description { get; set; }
        }
    }
}