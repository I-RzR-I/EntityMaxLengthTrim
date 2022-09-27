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

#endregion

namespace EntityMaxLengthTrim.Attributes
{
    /// <summary>
    ///     Maximum allowed string length attribute
    /// </summary>
    /// <remarks></remarks>
    [AttributeUsage(AttributeTargets.Property)]
    public class MaxAllowedLengthAttribute : Attribute
    {
        /// <summary>
        ///     Gets or sets maximum allows string length.
        /// </summary>
        /// <value></value>
        /// <remarks></remarks>
        public int MaxLength { get; set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="EntityMaxLengthTrim.Attributes.MaxAllowedLengthAttribute" /> class.
        /// </summary>
        /// <param name="maxLength">Maximum allows string length</param>
        /// <remarks></remarks>
        public MaxAllowedLengthAttribute(int maxLength)
        {
            MaxLength = maxLength;
        }
    }
}