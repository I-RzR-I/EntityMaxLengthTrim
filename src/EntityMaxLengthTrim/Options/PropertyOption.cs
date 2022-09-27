// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.EntityMaxLengthTrim
//  Author           : RzR
//  Created On       : 2022-09-26 06:35
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-09-26 06:38
// ***********************************************************************
//  <copyright file="PropertyOption.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

namespace EntityMaxLengthTrim.Options
{
    /// <summary>
    ///     Property option
    /// </summary>
    /// <remarks></remarks>
    public class PropertyOption
    {
        /// <summary>
        ///     Gets or sets property name.
        /// </summary>
        /// <value></value>
        /// <remarks></remarks>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether use dots on the end of string.
        /// </summary>
        /// <value>
        ///     <see langword="true" /> if at the end of the string will be ste (...); otherwise, <see langword="false" />.
        /// </value>
        /// <remarks></remarks>
        public bool UseDots { get; set; }
    }
}