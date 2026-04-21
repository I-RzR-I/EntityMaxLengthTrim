// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.EntityModelStringTruncateTest
//  Author           : RzR
//  Created On       : 2026-04-21 14:04
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-04-21 14:10
// ***********************************************************************
//  <copyright file="EntityPropChangeEventBaseTests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.ComponentModel.DataAnnotations;
using NUnit.Framework;
using RzR.Extensions.EntityLength;
using RzR.Extensions.EntityLength.Enums;

#endregion

namespace EntityModelStringTruncateTest
{
    public class EntityPropChangeEventBaseTests
    {
        [Test]
        public void Explicit_Length_Uses_Provided_Length_Not_Attributes()
        {
            var model = new ManualLengthInterceptModel
            {
                Value = "0123456789"
            };

            Assert.AreEqual(5, model.Value.Length);
            Assert.AreEqual("56789", model.Value);
        }

        [Test]
        public void Explicit_Length_AtStart_Does_Not_Throw_When_Setting_Null()
        {
            var model = new ManualLengthInterceptModel
            {
                Value = "0123456789"
            };

            Assert.DoesNotThrow(() => model.Value = null);
            Assert.IsNull(model.Value);
        }

        private class ManualLengthInterceptModel : EntityPropChangeEventBase
        {
            private string _value;

            [MaxLength(50)]
            public string Value
            {
                get => _value;
                set => SetContent(this, nameof(Value), ref _value, ref value, 5, StringTruncateType.AtTheStartOf);
            }
        }
    }
}