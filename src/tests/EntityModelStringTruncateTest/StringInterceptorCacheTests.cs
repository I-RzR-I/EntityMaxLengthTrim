// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.EntityModelStringTruncateTest
//  Author           : RzR
//  Created On       : 2026-04-21 14:04
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-04-21 14:52
// ***********************************************************************
//  <copyright file="StringInterceptorCacheTests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using NUnit.Framework;
using RzR.Extensions.EntityLength.Interceptors;

#endregion

namespace EntityModelStringTruncateTest
{
    public class StringInterceptorCacheTests
    {
        [Test]
        public void Zero_MaxLength_Is_Treated_As_No_Limit()
        {
            var model = new ZeroLengthModel { Name = "anything" };

            var result = StringInterceptor.ApplyStringMaxAllowedLength(model);

            Assert.AreEqual("anything", result.Name);
        }

        [Test]
        public void ReadOnly_String_Property_Is_Skipped()
        {
            var model = new ReadOnlyPropModel("very long backing value");

            var result = StringInterceptor.ApplyStringMaxAllowedLength(model);

            // No exception and original get-only property remains untouched.
            Assert.AreEqual("very long backing value", result.ReadOnly);
        }

        [Test]
        public void Interceptor_Is_Thread_Safe_Under_Concurrent_Access()
        {
            Parallel.For(0, 200, i =>
            {
                var a = new ParallelModelA { Name = "0123456789ABCDEF" };
                var b = new ParallelModelB { Title = "0123456789ABCDEFG" };

                StringInterceptor.ApplyStringMaxAllowedLength(a);
                StringInterceptor.ApplyStringMaxAllowedLength(b);

                Assert.AreEqual(10, a.Name.Length);
                Assert.AreEqual(12, b.Title.Length);
            });
        }

        private class ZeroLengthModel
        {
            [MaxLength(0)] 
            public string Name { get; set; }
        }

        private class ReadOnlyPropModel
        {
            public ReadOnlyPropModel(string value)
            {
                ReadOnly = value;
            }

            [MaxLength(5)] 
            public string ReadOnly { get; }
        }

        private class ParallelModelA
        {
            [MaxLength(10)] 
            public string Name { get; set; }
        }

        private class ParallelModelB
        {
            [MaxLength(12)]
            public string Title { get; set; }
        }
    }
}