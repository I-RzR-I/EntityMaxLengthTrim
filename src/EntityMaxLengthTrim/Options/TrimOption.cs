// ***********************************************************************
//  Assembly         : RzR.Extensions.EntityLength
//  Author           : RzR
//  Created On       : 2025-12-02 19:12
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-12-02 19:41
// ***********************************************************************
//  <copyright file="TrimOption.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using RzR.Extensions.EntityLength.Enums;

#endregion

namespace RzR.Extensions.EntityLength.Options
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     A trim option.
    /// </summary>
    /// =================================================================================================
    public class TrimOption
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the policy.
        /// </summary>
        /// <value>
        ///     The policy.
        /// </value>
        /// =================================================================================================
        public TrimPolicy Policy { get; set; } = TrimPolicy.Silent;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the logger.
        /// </summary>
        /// <value>
        ///     The logger.
        /// </value>
        /// =================================================================================================
        public Action<TrimContext> Logger { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets a value indicating whether this object use dots.
        /// </summary>
        /// <value>
        ///     True if use dots, false if not.
        /// </value>
        /// =================================================================================================
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
        ///     Gets or sets a value indicating whether trailing whitespace is trimmed
        ///     from the retained substring before returning the final value.
        /// </summary>
        /// <value>
        ///     True to apply TrimEnd on the retained substring;
        ///     false to preserve trailing spaces.
        ///     Default value is <see langword="true" />.
        /// </value>
        public bool ApplyForceTrimEnd { get; set; } = true;
    }
}