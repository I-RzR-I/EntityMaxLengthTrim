// ***********************************************************************
//  Assembly         : RzR.Extensions.EntityLength
//  Author           : RzR
//  Created On       : 2026-04-21 09:04
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-04-21 09:54
// ***********************************************************************
//  <copyright file="EntityMaxLengthExceededException.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using RzR.Extensions.EntityLength.Options;

#endregion

namespace RzR.Extensions.EntityLength.Exceptions
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Exception thrown when a string property value exceeds its maximum allowed length and the
    ///     active <see cref="Enums.TrimPolicy" /> is <see cref="Enums.TrimPolicy.Throw" />.
    /// </summary>
    /// <remarks>
    ///     The associated <see cref="TrimContext" /> describes the entity, property name, original value
    ///     and configured limit so callers can produce actionable diagnostics.
    /// </remarks>
    /// =================================================================================================
    public class EntityMaxLengthExceededException : Exception
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="EntityMaxLengthExceededException" /> class.
        /// </summary>
        /// <param name="context">The trim context describing the offending property.</param>
        public EntityMaxLengthExceededException(TrimContext context)
            : base(BuildMessage(context))
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="EntityMaxLengthExceededException" /> class.
        /// </summary>
        /// <param name="context">The trim context describing the offending property.</param>
        /// <param name="message">A custom error message.</param>
        public EntityMaxLengthExceededException(TrimContext context, string message)
            : base(message)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="EntityMaxLengthExceededException" /> class.
        /// </summary>
        /// <param name="context">The trim context describing the offending property.</param>
        /// <param name="message">A custom error message.</param>
        /// <param name="innerException">The inner exception.</param>
        public EntityMaxLengthExceededException(TrimContext context, string message, Exception innerException)
            : base(message, innerException)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        ///     Gets the trim context describing the offending property.
        /// </summary>
        public TrimContext Context { get; }

        private static string BuildMessage(TrimContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            return $"Property '{context.PropertyName}' on entity '{context.Entity}' " +
                   $"exceeded max length {context.MaxLength} (original length {context.OriginalValue?.Length ?? 0}).";
        }
    }
}