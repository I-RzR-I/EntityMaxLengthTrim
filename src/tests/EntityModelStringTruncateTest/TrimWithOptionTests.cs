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
using System;
using EntityModelStringTruncateTest.Helpers;
using RzR.Extensions.EntityLength.Enums;
using RzR.Extensions.EntityLength.Exceptions;
using RzR.Extensions.EntityLength.Interceptors;
using RzR.Extensions.EntityLength.Options;

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

        [Test]
        public void Throw_Policy_Throws_Typed_Exception_With_Context()
        {
            var ex = Assert.Throws<EntityMaxLengthExceededException>(() =>
                StringInterceptor.ApplyStringMaxAllowedLength(_test1Model, new TrimOption
                {
                    Policy = TrimPolicy.Throw
                }));

            Assert.IsNotNull(ex.Context);
            Assert.AreEqual(nameof(FooModel), ex.Context.Entity);
            Assert.AreEqual(nameof(FooModel.Name), ex.Context.PropertyName);
            Assert.AreEqual(PropertyMaxLengthHelper.NameMaxLength, ex.Context.MaxLength);
            Assert.AreEqual(Constants.TextWithLength35, ex.Context.OriginalValue);

            // Entity must remain unchanged when the throw policy fires (fail-fast, no truncation).
            Assert.AreEqual(Constants.TextWithLength35, _test1Model.Name);
        }

        [Test]
        public void Throw_Policy_Does_Not_Throw_When_All_Properties_Within_Limit()
        {
            var model = new FooModel { Name = "ok", FullName = "ok", Description = "ok" };

            Assert.DoesNotThrow(() =>
                StringInterceptor.ApplyStringMaxAllowedLength(model, new TrimOption
                {
                    Policy = TrimPolicy.Throw
                }));

            Assert.AreEqual("ok", model.Name);
        }

        [TestCase(1, ".")]
        [TestCase(2, "..")]
        [TestCase(3, "...")]
        public void UseDots_AtTheEnd_WithVerySmallMaxLength_Still_Respects_MaxLength(int maxLength, string expected)
        {
            var result = ApplySmallLimit(maxLength, StringTruncateType.AtTheEndOf);

            Assert.AreEqual(expected, result);
            Assert.AreEqual(maxLength, result.Length);
        }

        [TestCase(1, ".")]
        [TestCase(2, "..")]
        [TestCase(3, "...")]
        public void UseDots_AtTheStart_WithVerySmallMaxLength_Still_Respects_MaxLength(int maxLength, string expected)
        {
            var result = ApplySmallLimit(maxLength, StringTruncateType.AtTheStartOf);

            Assert.AreEqual(expected, result);
            Assert.AreEqual(maxLength, result.Length);
        }

        [Test]
        public void Truncate_WithoutDots_Preserves_Trailing_Spaces_In_Retained_Slice()
        {
            var model = new MaxLengthFiveModel { Text = "abc  xyz" };

            var result = StringInterceptor.ApplyStringMaxAllowedLength(model, new TrimOption
            {
                Policy = TrimPolicy.Silent,
                UseDots = false,
                ApplyForceTrimEnd = false
            });

            Assert.AreEqual("abc  ", result.Text);
            Assert.AreEqual(5, result.Text.Length);
        }

        [Test]
        public void Truncate_WithDots_Trims_Only_The_Space_Before_Suffix()
        {
            var model = new MaxLengthSixModel { Text = "ab  xyz" };

            var result = StringInterceptor.ApplyStringMaxAllowedLength(model, new TrimOption
            {
                Policy = TrimPolicy.Silent,
                UseDots = true,
                ApplyForceTrimEnd = true
            });

            Assert.AreEqual("ab...", result.Text);
            Assert.AreEqual(5, result.Text.Length);
        }

        [Test]
        public void Truncate_WithDots_ApplyForceTrimEndFalse_Preserves_Space_Before_Suffix()
        {
            var model = new MaxLengthSixModel { Text = "ab  xyz" };

            var result = StringInterceptor.ApplyStringMaxAllowedLength(model, new TrimOption
            {
                Policy = TrimPolicy.Silent,
                UseDots = true,
                ApplyForceTrimEnd = false
            });

            Assert.AreEqual("ab ...", result.Text);
            Assert.AreEqual(6, result.Text.Length);
        }

        [Test]
        public void TruncateAtStart_WithoutDots_ApplyForceTrimEndTrue_Preserves_Trailing_Spaces()
        {
            var model = new MaxLengthFiveModel { Text = "abcd  " };

            var result = StringInterceptor.ApplyStringMaxAllowedLength(model, new TrimOption
            {
                Policy = TrimPolicy.Silent,
                UseDots = false,
                TruncateType = StringTruncateType.AtTheStartOf,
                ApplyForceTrimEnd = true
            });

            Assert.AreEqual("bcd  ", result.Text);
            Assert.AreEqual(5, result.Text.Length);
        }

        [Test]
        public void TruncateAtStart_WithoutDots_ApplyForceTrimEndFalse_Preserves_Trailing_Spaces()
        {
            var model = new MaxLengthFiveModel { Text = "abcd  " };

            var result = StringInterceptor.ApplyStringMaxAllowedLength(model, new TrimOption
            {
                Policy = TrimPolicy.Silent,
                UseDots = false,
                TruncateType = StringTruncateType.AtTheStartOf,
                ApplyForceTrimEnd = false
            });

            Assert.AreEqual("bcd  ", result.Text);
            Assert.AreEqual(5, result.Text.Length);
        }

        [Test]
        public void Null_Entity_Throws_ArgumentNullException_Even_With_Reflection_Errors_No_Longer_Swallowed()
        {
            Assert.Throws<ArgumentNullException>(() =>
                StringInterceptor.ApplyStringMaxAllowedLength<FooModel>(null, new TrimOption()));
        }

        private static string ApplySmallLimit(int maxLength, StringTruncateType truncateType)
        {
            var options = new TrimOption
            {
                Policy = TrimPolicy.Silent,
                UseDots = true,
                TruncateType = truncateType
            };

            switch (maxLength)
            {
                case 1:
                    return StringInterceptor.ApplyStringMaxAllowedLength(new SmallLimit1Model { Text = "abcdef" }, options).Text;
                case 2:
                    return StringInterceptor.ApplyStringMaxAllowedLength(new SmallLimit2Model { Text = "abcdef" }, options).Text;
                case 3:
                    return StringInterceptor.ApplyStringMaxAllowedLength(new SmallLimit3Model { Text = "abcdef" }, options).Text;
                default:
                    throw new ArgumentOutOfRangeException(nameof(maxLength));
            }
        }
    }

    class Utf8Test
    {
        [System.ComponentModel.DataAnnotations.MaxLength(100)]
        public string Text { get; set; } = "😀😀😀";
    }

    class SmallLimit1Model
    {
        [System.ComponentModel.DataAnnotations.MaxLength(1)]
        public string Text { get; set; }
    }

    class SmallLimit2Model
    {
        [System.ComponentModel.DataAnnotations.MaxLength(2)]
        public string Text { get; set; }
    }

    class SmallLimit3Model
    {
        [System.ComponentModel.DataAnnotations.MaxLength(3)]
        public string Text { get; set; }
    }

    class MaxLengthFiveModel
    {
        [System.ComponentModel.DataAnnotations.MaxLength(5)]
        public string Text { get; set; }
    }

    class MaxLengthSixModel
    {
        [System.ComponentModel.DataAnnotations.MaxLength(6)]
        public string Text { get; set; }
    }
}