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
using EntityModelStringTruncateTest.Helpers;
using RzR.Extensions.EntityLength;
using RzR.Extensions.EntityLength.Attributes;

namespace EntityModelStringTruncateTest.Models
{
    public class FooModelIntercept : EntityPropChangeEventBase
    {
        private string _name;
        private string _fullName;
        private string _description;

        [MaxLength(PropertyMaxLengthHelper.NameMaxLength)]
        public string Name
        {
            get => _name;
            set
            {
                if (_name == value) return;
                _name = value;
                OnPropertyChanged(this, nameof(Name));
            }
        }

        [StringLength(PropertyMaxLengthHelper.FullNameMaxLength)]
        public string FullName
        {
            get => _fullName;
            set
            {
                if (_fullName == value) return;
                _fullName = value;
                OnPropertyChanged(this, nameof(FullName));
            }
        }

        [MaxAllowedLength(PropertyMaxLengthHelper.DescriptionMaxLength)]
        public string Description
        {
            get => _description;
            set
            {
                if (_description == value) return;
                _description = value;
                OnPropertyChanged(this, nameof(Description));
            }
        }
    }
}