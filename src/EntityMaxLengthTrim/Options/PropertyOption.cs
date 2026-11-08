// ***********************************************************************
//  Assembly         : RzR.Extensions.EntityLength
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

#region U S A G E S

using RzR.Extensions.EntityLength.Enums;

// ReSharper disable ClassNeverInstantiated.Global

#endregion

namespace RzR.Extensions.EntityLength.Options
{
    /// <summary>
    ///     Property option
    /// </summary>
    /// <remarks>Options used to specify truncate details</remarks>
    public class PropertyOption
    {
        /// <summary>
        ///     Gets or sets property name.
        /// </summary>
        /// <value>Name of property to be truncated (string property name)</value>
        /// <remarks></remarks>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether to use dots on the end of the string.
        /// </summary>
        /// <value>
        ///     <see langword="true" /> if at the end of the string will be set (...); otherwise, <see langword="false" />.
        /// </value>
        /// <remarks>Bool value</remarks>
        public bool UseDots { get; set; }

        /// <summary>
        ///     Gets or sets the type of the truncate.
        /// </summary>
        /// <value>
        ///     The type of the truncate.
        ///     Default value is truncate at the end of the string.
        /// </value>
        public StringTruncateType TruncateType { get; set; } = StringTruncateType.AtTheEndOf;

        /// <summary>
        ///     Gets or sets a value indicating whether trailing white-space is removed from the retained part
        ///     after an over-length value is cut, for this property.
        /// </summary>
        /// <value>
        ///     When <see langword="true" />, trailing white-space (<c>char.IsWhiteSpace</c>, including CR/LF and tabs)
        ///     is removed from the retained part, for both truncation directions and with or without dots;
        ///     for <c>AtTheStartOf</c> this is the end of the original value.
        ///     When <see langword="false" />, the retained part is kept verbatim.
        ///     When <see langword="null" />, the method-level value is used.
        ///     Values within the limit are never modified.
        /// </value>
        public bool? ApplyForceTrimEnd { get; set; }
    }
}