// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.ConsoleFrameworkTest
//  Author           : RzR
//  Created On       : 2022-09-26 22:53
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-09-26 22:58
// ***********************************************************************
//  <copyright file="Foo.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.ComponentModel.DataAnnotations;
using ConsoleFrameworkTest.Helpers;
using EntityMaxLengthTrim.Attributes;

#endregion

namespace ConsoleFrameworkTest.Models
{
    public class Foo
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        [MaxLength(PropertyMaxLengthHelper.NameMaxLength)]
        public string Name { get; set; }

        [StringLength(PropertyMaxLengthHelper.FullNameMaxLength)]
        public string FullName { get; set; }

        [MaxAllowedLength(PropertyMaxLengthHelper.DescriptionMaxLength)]
        public string Description { get; set; }
    }
}