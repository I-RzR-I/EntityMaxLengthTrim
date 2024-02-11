// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.EntityModelStringTruncateTest
//  Author           : RzR
//  Created On       : 2023-04-07 12:39
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-04-07 12:39
// ***********************************************************************
//  <copyright file="FooModelIntercept.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;
using EntityMaxLengthTrim;
using EntityMaxLengthTrim.Attributes;
using EntityModelStringTruncateTest.Helpers;

namespace EntityModelStringTruncateTest.Models
{
    public class FooModelIntercept2 : EntityPropChangeEventBase
    {
        private string _name;
        private string _fullName;
        private string _description;

        [MaxLength(PropertyMaxLengthHelper.NameMaxLength)]
        public string Name
        {
            get => _name;
            set => SetContent(this, nameof(Name), ref _name, ref value);
        }

        [StringLength(PropertyMaxLengthHelper.FullNameMaxLength)]
        public string FullName
        {
            get => _fullName;
            set => SetContent(this, nameof(FullName), ref _fullName, ref value);
        }

        public string Description
        {
            get => _description;
            set => SetContent(this, nameof(Description), ref _description, ref value, PropertyMaxLengthHelper.DescriptionMaxLength);
        }
    }
}