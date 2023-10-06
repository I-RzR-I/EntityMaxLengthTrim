// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.TruncateEntityModelString2Test
//  Author           : RzR
//  Created On       : 2023-10-06 22:43
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-10-07 00:03
// ***********************************************************************
//  <copyright file="TruncateEntityModelString2Test.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Collections.Generic;
using EntityMaxLengthTrim.Extensions;
using EntityMaxLengthTrim.Options;
using EntityModelStringTruncateTest.Helpers;
using EntityModelStringTruncateTest.Models;
using NUnit.Framework;

#endregion

namespace EntityModelStringTruncateTest
{
    public class TruncateEntityModelString2Test
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
        public void Test1()
        {
            var parsedData = _test1Model.ToSafeStoreStrings();

            Assert.IsEmpty(parsedData.FullName);

            Assert.IsNotNull(parsedData.Name);
            Assert.AreEqual(PropertyMaxLengthHelper.NameMaxLength, parsedData.Name.Length);

            Assert.IsNull(parsedData.Description);
        }

        [Test]
        public void Test2()
        {
            var parsedData = _test2Model.ToSafeStoreStrings(true);

            Assert.IsNotNull(parsedData.Name);
            Assert.AreEqual(PropertyMaxLengthHelper.NameMaxLength, parsedData.Name.Length);
            Assert.IsTrue(parsedData.Name.EndsWith("..."));

            Assert.IsEmpty(parsedData.FullName);

            Assert.IsNull(parsedData.Description);
        }

        [Test]
        public void Test3()
        {
            var parsedData = _test3Model.ToSafeStoreStrings(new List<string> {nameof(FooModel.Name)});

            Assert.IsNotNull(parsedData.Name);
            Assert.AreEqual(PropertyMaxLengthHelper.NameMaxLength, parsedData.Name.Length);
            Assert.IsTrue(parsedData.Name.EndsWith("..."));

            Assert.IsEmpty(parsedData.FullName);

            Assert.IsNull(parsedData.Description);
        }

        [Test]
        public void Test4()
        {
            var parsedData = _test4Model.ToSafeStoreStrings(new List<string> {nameof(FooModel.Name)}, true);

            Assert.IsNotNull(parsedData.Name);
            Assert.AreEqual(PropertyMaxLengthHelper.NameMaxLength, parsedData.Name.Length);
            Assert.IsTrue(parsedData.Name.EndsWith("..."));

            Assert.IsEmpty(parsedData.FullName);

            Assert.IsNotNull(parsedData.Description);
            Assert.AreEqual(_test4Model.Description.Length, parsedData.Description.Length);
            Assert.IsFalse(parsedData.Description.EndsWith("..."));
        }

        [Test]
        public void Test5()
        {
            var parsedData = _test5Model.ToSafeStoreStrings(
                new List<PropertyOption>
                {
                    new PropertyOption {Name = nameof(FooModel.Name), UseDots = false},
                    new PropertyOption {Name = nameof(FooModel.Description), UseDots = true}
                });

            Assert.IsNotNull(parsedData.Name);
            Assert.AreEqual(PropertyMaxLengthHelper.NameMaxLength, parsedData.Name.Length);
            Assert.IsFalse(parsedData.Name.EndsWith("..."));

            Assert.AreEqual(PropertyMaxLengthHelper.FullNameMaxLength, parsedData.FullName.Length);
            Assert.IsFalse(parsedData.FullName.EndsWith("..."));

            Assert.IsNotNull(parsedData.Description);
            Assert.AreEqual(PropertyMaxLengthHelper.DescriptionMaxLength, parsedData.Description.Length);
            Assert.IsTrue(parsedData.Description.EndsWith("..."));
        }

        [Test]
        public void Test6()
        {
            var parsedData = _test6Model.ToSafeStoreStrings(
                new List<PropertyOption>
                {
                    new PropertyOption {Name = nameof(FooModel.Name), UseDots = false},
                    new PropertyOption {Name = nameof(FooModel.Description), UseDots = true}
                }, true);

            Assert.IsNotNull(parsedData.Name);
            Assert.AreEqual(PropertyMaxLengthHelper.NameMaxLength, parsedData.Name.Length);
            Assert.IsFalse(parsedData.Name.EndsWith("..."));

            Assert.AreEqual(_test6Model.FullName.Length, parsedData.FullName.Length);
            Assert.IsFalse(parsedData.FullName.EndsWith("..."));

            Assert.IsNotNull(parsedData.Description);
            Assert.AreEqual(PropertyMaxLengthHelper.DescriptionMaxLength, parsedData.Description.Length);
            Assert.IsTrue(parsedData.Description.EndsWith("..."));
        }

        [Test]
        public void Test7()
        {
            var data = new FooModelIntercept
            {
                Name = Constants.TextWithLength35,
                FullName = Constants.TextWithLength35,
                Description = Constants.TextWithLength575
            };

            Assert.IsTrue(PropertyMaxLengthHelper.NameMaxLength >= data.Name.Length);
            Assert.IsTrue(PropertyMaxLengthHelper.FullNameMaxLength >= data.FullName.Length);
            Assert.IsTrue(PropertyMaxLengthHelper.DescriptionMaxLength >= data.Description.Length);
        }
    }
}