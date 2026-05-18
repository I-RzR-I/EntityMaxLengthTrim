// ***********************************************************************
//  Assembly         : RzR.Extensions.EntityLength
//  Author           : RzR
//  Created On       : 2023-10-04 12:33
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-10-04 17:50
// ***********************************************************************
//  <copyright file="IntExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

namespace RzR.Extensions.EntityLength.Extensions.Internal
{
    /// <summary>
    ///     Int32 extensions
    /// </summary>
    internal static class IntExtensions
    {
        /// <summary>
        ///     Check if source object is null or equals with 0
        /// </summary>
        /// <param name="source">Source object to be checked</param>
        /// <remarks>
        ///     Source: https://github.com/I-RzR-I/DomainCommonExtensions
        /// </remarks>
        /// <returns>Return bool value (validation result).</returns>
        internal static bool IsNullOrZero(this int source) => source.IsNull() || source == 0;

        /// <summary>
        ///     Check if source object is less or equals with 0
        /// </summary>
        /// <param name="source">Source object to be checked</param>
        /// <remarks>
        ///     Source: https://github.com/I-RzR-I/DomainCommonExtensions
        /// </remarks>
        /// <returns>Return bool value (validation result).</returns>
        internal static bool IsLessOrEqualWithZero(this int source) => source <= 0;
    }
}