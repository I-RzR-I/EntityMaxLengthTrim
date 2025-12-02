// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.EntityMaxLengthTrim
//  Author           : RzR
//  Created On       : 2025-12-02 19:12
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-12-02 19:39
// ***********************************************************************
//  <copyright file="TrimContext.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;

#endregion

namespace EntityMaxLengthTrim.Options
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     A trim context.
    /// </summary>
    /// =================================================================================================
    public class TrimContext
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the entity.
        /// </summary>
        /// <value>
        ///     The entity.
        /// </value>
        /// =================================================================================================
        public string Entity { get; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the name of the property.
        /// </summary>
        /// <value>
        ///     The name of the property.
        /// </value>
        /// =================================================================================================
        public string PropertyName { get; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the original value.
        /// </summary>
        /// <value>
        ///     The original value.
        /// </value>
        /// =================================================================================================
        public string OriginalValue { get; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the result value.
        /// </summary>
        /// <value>
        ///     The result value.
        /// </value>
        /// =================================================================================================
        public string ResultValue { get; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the maximum length.
        /// </summary>
        /// <value>
        ///     The maximum allowed length.
        /// </value>
        /// =================================================================================================
        public int? MaxLength { get; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="TrimContext"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="entity">The entity.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="original">The original.</param>
        /// <param name="result">The result.</param>
        /// <param name="maxLength">The maximum allowed length.</param>
        /// =================================================================================================
        public TrimContext(string entity, string propertyName, string original, string result, int? maxLength)
        {
            Entity = entity ?? throw new ArgumentNullException(nameof(entity));
            PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            OriginalValue = original;
            ResultValue = result;
            MaxLength = maxLength;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"Trim: Entity={Entity}, Property={PropertyName}, MaxLength={MaxLength}," +
                   $" OriginalLength={OriginalValue?.Length ?? 0}, ResultLength={ResultValue?.Length ?? 0}";
        }
    }
}