// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.EntityModelStringTruncateTest
//  Author           : RzR
//  Created On       : 2022-09-26 18:30
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-09-26 21:36
// ***********************************************************************
//  <copyright file="TruncateEntityModelStringTest.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Collections.Generic;
using EntityMaxLengthTrim.Interceptors;
using EntityMaxLengthTrim.Options;
using EntityModelStringTruncateTest.Helpers;
using EntityModelStringTruncateTest.Models;
using NUnit.Framework;

#endregion

namespace EntityModelStringTruncateTest
{
    public class TruncateEntityModelStringTest
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
                Name = "Here should be the new name of FOO",
                FullName = string.Empty,
                Description = null
            };

            _test2Model = new FooModel
            {
                Name = "Here should be the new name of FOO",
                FullName = string.Empty,
                Description = null
            };

            _test3Model = new FooModel
            {
                Name = "Here should be the new name of FOO",
                FullName = string.Empty,
                Description = null
            };

            _test4Model = new FooModel
            {
                Name = "Here should be the new name of FOO",
                FullName = string.Empty,
                Description =
                    "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum."
            };

            _test5Model = new FooModel
            {
                Name = "Here should be the new name of FOO",
                FullName =
                    "It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout. The point of using Lorem Ipsum is that it has a more-or-less normal distribution of letters, as opposed to using 'Content here, content here', making it look like readable English. Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                Description =
                    "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum."
            };

            _test6Model = new FooModel
            {
                Name = "Here should be the new name of FOO",
                FullName =
                    "It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout. The point of using Lorem Ipsum is that it has a more-or-less normal distribution of letters, as opposed to using 'Content here, content here', making it look like readable English. Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                Description =
                    "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum."
            };
        }

        [Test]
        public void Test1()
        {
            var parsedData = StringInterceptor.ApplyStringMaxAllowedLength(_test1Model);

            Assert.IsEmpty(parsedData.FullName);

            Assert.IsNotNull(parsedData.Name);
            Assert.AreEqual(PropertyMaxLengthHelper.NameMaxLength, parsedData.Name.Length);

            Assert.IsNull(parsedData.Description);
        }

        [Test]
        public void Test2()
        {
            var parsedData = StringInterceptor.ApplyStringMaxAllowedLength(_test2Model, true);

            Assert.IsNotNull(parsedData.Name);
            Assert.AreEqual(PropertyMaxLengthHelper.NameMaxLength, parsedData.Name.Length);
            Assert.IsTrue(parsedData.Name.EndsWith("..."));

            Assert.IsEmpty(parsedData.FullName);

            Assert.IsNull(parsedData.Description);
        }

        [Test]
        public void Test3()
        {
            var parsedData =
                StringInterceptor.ApplyStringMaxAllowedLength(_test3Model, new List<string> {nameof(FooModel.Name)});

            Assert.IsNotNull(parsedData.Name);
            Assert.AreEqual(PropertyMaxLengthHelper.NameMaxLength, parsedData.Name.Length);
            Assert.IsTrue(parsedData.Name.EndsWith("..."));

            Assert.IsEmpty(parsedData.FullName);

            Assert.IsNull(parsedData.Description);
        }

        [Test]
        public void Test4()
        {
            var parsedData =
                StringInterceptor.ApplyStringMaxAllowedLength(_test4Model, new List<string> {nameof(FooModel.Name)},
                    true);

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
            var parsedData = StringInterceptor.ApplyStringMaxAllowedLength(_test5Model,
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
            var parsedData = StringInterceptor.ApplyStringMaxAllowedLength(_test6Model,
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
    }
}