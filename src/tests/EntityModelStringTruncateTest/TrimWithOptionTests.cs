// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.EntityModelStringTruncateTest
//  Author           : RzR
//  Created On       : 2025-12-02 21:12
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-12-02 21:18
// ***********************************************************************
//  <copyright file="TrimWithOptionTests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using EntityModelStringTruncateTest.Models;
using NUnit.Framework;
using System.Collections.Generic;
using EntityMaxLengthTrim.Enums;
using EntityMaxLengthTrim.Extensions;
using EntityMaxLengthTrim.Interceptors;
using EntityMaxLengthTrim.Options;
using EntityModelStringTruncateTest.Helpers;

#endregion

namespace EntityModelStringTruncateTest
{
    public class TrimWithOptionTests
    {
        private FooModel _test1Model;
        private FooModel _test2Model;
        private FooModel _test3Model;
        private FooModel _test4Model;
        private FooModel _test5Model;
        private FooModel _test6Model;

        [SetUp]
        public void Setup()
        {
            _test1Model = new FooModel
            {
                Name = Constants.TextWithLength35,
                FullName = string.Empty,
                Description = null
            };

            _test2Model = new FooModel
            {
                Name = Constants.TextWithLength35,
                FullName = string.Empty,
                Description = null
            };

            _test3Model = new FooModel
            {
                Name = Constants.TextWithLength35,
                FullName = string.Empty,
                Description = null
            };

            _test4Model = new FooModel
            {
                Name = Constants.TextWithLength35,
                FullName = string.Empty,
                Description = Constants.TextWithLength575
            };

            _test5Model = new FooModel
            {
                Name = Constants.TextWithLength35,
                FullName = Constants.TextWithLength575,
                Description = Constants.TextWithLength575
            };

            _test6Model = new FooModel
            {
                Name = Constants.TextWithLength35,
                FullName = Constants.TextWithLength575,
                Description = Constants.TextWithLength575
            };
        }

        [Test]
        public void Test_1()
        {
            var parsedData = StringInterceptor.ApplyStringMaxAllowedLength(_test1Model, new TrimOption()
            {
                UseDots = false,
                TruncateType = StringTruncateType.AtTheEndOf,
                Policy = TrimPolicy.Silent,
                Logger = context =>
                {
                    Assert.IsNotEmpty(context.Entity);
                    Assert.IsNotEmpty(context.PropertyName);
                }
            });

            Assert.IsNotNull(parsedData.Name);
            Assert.AreEqual(PropertyMaxLengthHelper.NameMaxLength, parsedData.Name.Length);
            Assert.IsFalse(parsedData.Name.EndsWith("..."));

            Assert.AreNotEqual(_test6Model.FullName.Length, parsedData.FullName.Length);
            Assert.IsEmpty(_test1Model.FullName);
            Assert.IsFalse(parsedData.FullName.EndsWith("..."));

            Assert.IsNull(parsedData.Description);
            Assert.AreNotEqual(PropertyMaxLengthHelper.DescriptionMaxLength, parsedData.Description?.Length);
        }

        [Test]
        public void Test_1X()
        {
            var reported = false;
            var parsedData = StringInterceptor.ApplyStringMaxAllowedLength(_test1Model, new TrimOption()
            {
                UseDots = false,
                TruncateType = StringTruncateType.AtTheEndOf,
                Policy = TrimPolicy.Warn,
                Logger = context =>
                {
                    reported = true;
                    Assert.IsNotEmpty(context.Entity);
                    Assert.IsNotEmpty(context.PropertyName);
                }
            });

            Assert.IsTrue(reported);

            Assert.IsNotNull(parsedData.Name);
            Assert.AreEqual(PropertyMaxLengthHelper.NameMaxLength, parsedData.Name.Length);
            Assert.IsFalse(parsedData.Name.EndsWith("..."));

            Assert.AreNotEqual(_test6Model.FullName.Length, parsedData.FullName.Length);
            Assert.IsEmpty(_test1Model.FullName);
            Assert.IsFalse(parsedData.FullName.EndsWith("..."));

            Assert.IsNull(parsedData.Description);
            Assert.AreNotEqual(PropertyMaxLengthHelper.DescriptionMaxLength, parsedData.Description?.Length);
        }

        [Test]
        public void Test_1Y()
        {
            var reported = false;
            var parsedData = StringInterceptor.ApplyStringMaxAllowedLength(_test1Model, new TrimOption()
            {
                UseDots = false,
                TruncateType = StringTruncateType.AtTheEndOf,
                Policy = TrimPolicy.ReportOnly,
                Logger = context =>
                {
                    reported = true;
                    Assert.IsNotEmpty(context.Entity);
                    Assert.IsNotEmpty(context.PropertyName);
                }
            });

            Assert.IsTrue(reported);

            Assert.IsNotNull(parsedData.Name);
            Assert.AreNotEqual(PropertyMaxLengthHelper.NameMaxLength, parsedData.Name.Length);
            Assert.AreEqual(_test1Model.Name.Length, parsedData.Name.Length);
            Assert.IsFalse(parsedData.Name.EndsWith("..."));

            Assert.AreNotEqual(_test6Model.FullName.Length, parsedData.FullName.Length);
            Assert.IsEmpty(_test1Model.FullName);
            Assert.IsFalse(parsedData.FullName.EndsWith("..."));

            Assert.IsNull(parsedData.Description);
            Assert.AreNotEqual(PropertyMaxLengthHelper.DescriptionMaxLength, parsedData.Description?.Length);
        }

        [Test]
        public void Test_X1()
        {
            var t = new Utf8Test { Text = "😀😀😀" };

            var result = StringInterceptor.ApplyStringMaxAllowedLength(t, new TrimOption()
            {
                Policy = TrimPolicy.Silent
            });

            Assert.AreEqual(6, result.Text?.Length);
            Assert.True(result.Text?.Length <= 100);
            Assert.True(t.Text.Length <= 100);
        }
    }

    class Utf8Test
    {
        [System.ComponentModel.DataAnnotations.MaxLength(100)]
        public string Text { get; set; } = "😀😀😀";
    }
}