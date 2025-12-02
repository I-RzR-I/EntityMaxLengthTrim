// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.EntityMaxLengthTrim
//  Author           : RzR
//  Created On       : 2023-10-04 17:29
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-10-04 17:50
// ***********************************************************************
//  <copyright file="ObjectExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using CodeSource;

#endregion

namespace EntityMaxLengthTrim.Extensions.Internal
{
    /// <summary>
    ///     Object extensions
    /// </summary>
    internal static class ObjectExtensions
    {
        /// <summary>
        ///     Check if the source object is null
        /// </summary>
        /// <param name="source">Object to be checked</param>
        /// <returns>Return bool value (validation result).</returns>
        [CodeSource("https://github.com/I-RzR-I/DomainCommonExtensions", "RzR", 1.0)]
        internal static bool IsNull(this object source) => source == null;

        /// <summary>
        ///     Check if the source object is not null
        /// </summary>
        /// <param name="source">Object to be checked</param>
        /// <returns>Return bool value (validation result).</returns>
        [CodeSource("https://github.com/I-RzR-I/DomainCommonExtensions", "RzR", 1.0)]
        internal static bool IsNotNull(this object source) => !source.IsNull();
    }
}