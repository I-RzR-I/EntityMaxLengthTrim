// ***********************************************************************
//  Assembly         : RzR.Extensions.EntityLength
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

namespace RzR.Extensions.EntityLength.Extensions.Internal
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
        /// <remarks>
        ///     Source: https://github.com/I-RzR-I/DomainCommonExtensions
        /// </remarks>
        /// <returns>Return bool value (validation result).</returns>
        internal static bool IsNull(this object source) => source == null;

        /// <summary>
        ///     Check if the source object is not null
        /// </summary>
        /// <param name="source">Object to be checked</param>
        /// <remarks>
        ///     Source: https://github.com/I-RzR-I/DomainCommonExtensions
        /// </remarks>
        /// <returns>Return bool value (validation result).</returns>
        internal static bool IsNotNull(this object source) => !source.IsNull();
    }
}