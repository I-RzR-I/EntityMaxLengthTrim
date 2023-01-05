// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.ConsoleFramework45Test
//  Author           : RzR
//  Created On       : 2023-01-05 09:53
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-01-05 09:59
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
using ConsoleFramework45Test.Helpers;
using EntityMaxLengthTrim.Attributes;

#endregion

namespace ConsoleFramework45Test.Models
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