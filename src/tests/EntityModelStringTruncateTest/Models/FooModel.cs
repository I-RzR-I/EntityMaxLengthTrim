// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.EntityModelStringTruncateTest
//  Author           : RzR
//  Created On       : 2022-09-26 18:24
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-09-26 21:36
// ***********************************************************************
//  <copyright file="FooModel.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.ComponentModel.DataAnnotations;
using EntityMaxLengthTrim.Attributes;
using EntityModelStringTruncateTest.Helpers;

#endregion

namespace EntityModelStringTruncateTest.Models
{
    public class FooModel
    {
        [MaxLength(PropertyMaxLengthHelper.NameMaxLength)]
        public string Name { get; set; }

        [StringLength(PropertyMaxLengthHelper.FullNameMaxLength)]
        public string FullName { get; set; }

        [MaxAllowedLength(PropertyMaxLengthHelper.DescriptionMaxLength)]
        public string Description { get; set; }
    }
}