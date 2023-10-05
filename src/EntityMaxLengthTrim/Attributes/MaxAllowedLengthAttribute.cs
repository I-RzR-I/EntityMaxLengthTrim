// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.EntityMaxLengthTrim
//  Author           : RzR
//  Created On       : 2022-09-24 02:47
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-09-24 03:37
// ***********************************************************************
//  <copyright file="MaxAllowedLengthAttribute.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using EntityMaxLengthTrim.Extensions;

#endregion

namespace EntityMaxLengthTrim.Attributes
{
    /// <summary>
    ///     Maximum allowed string length attribute
    /// </summary>
    /// <remarks>Current property is allowed only for property decoration</remarks>
    [AttributeUsage(AttributeTargets.Property)]
    public class MaxAllowedLengthAttribute : Attribute
    {
        /// <summary>
        ///     Gets or sets maximum allows string length.
        /// </summary>
        /// <value>Maximum allowed length of property</value>
        /// <remarks>Allowed property type is 'Int32' not nullable</remarks>
        public int MaxLength { get; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="EntityMaxLengthTrim.Attributes.MaxAllowedLengthAttribute" /> class.
        /// </summary>
        /// <param name="maxLength">Maximum allows string length</param>
        /// <remarks>Initialize property maximum length</remarks>
        public MaxAllowedLengthAttribute(int maxLength)
        {
            if(maxLength.IsNullOrZero())
                throw new ArgumentException($"The {nameof(maxLength)} must be greater then 0!");

            MaxLength = maxLength;
        }
    }
}