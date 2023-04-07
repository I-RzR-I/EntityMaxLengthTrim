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

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using EntityMaxLengthTrim.Attributes;
using EntityMaxLengthTrim.Interceptors;
using EntityModelStringTruncateTest.Helpers;

namespace EntityModelStringTruncateTest.Models
{
    public class FooModelIntercept : INotifyPropertyChanged
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
                OnPropertyChanged(nameof(Name));
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
                OnPropertyChanged(nameof(FullName));
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
                OnPropertyChanged(nameof(Description));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            StringInterceptor.ApplyStringMaxAllowedLength(this, propertyName, false);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}