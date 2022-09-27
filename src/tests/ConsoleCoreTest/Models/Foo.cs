// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.ConsoleCoreTest
//  Author           : RzR
//  Created On       : 2022-09-24 03:08
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-09-24 03:08
// ***********************************************************************
//  <copyright file="Foo.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

using System;
using System.ComponentModel.DataAnnotations;
using ConsoleCoreTest.Helpers;
using EntityMaxLengthTrim.Attributes;

namespace ConsoleCoreTest.Models
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