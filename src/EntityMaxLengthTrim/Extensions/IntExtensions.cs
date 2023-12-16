// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.EntityMaxLengthTrim
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

#region U S A G E S

using CodeSource;

#endregion

namespace EntityMaxLengthTrim.Extensions
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
        /// <returns>Return bool value (validation result).</returns>
        [CodeSource("https://github.com/I-RzR-I/DomainCommonExtensions", "RzR", 1.0)]
        internal static bool IsNullOrZero(this int source)
        {
            return source.IsNull() || source == 0;
        }

        /// <summary>
        ///     Check if source object is less or equals with 0
        /// </summary>
        /// <param name="source">Source object to be checked</param>
        /// <returns>Return bool value (validation result).</returns>
        [CodeSource("https://github.com/I-RzR-I/DomainCommonExtensions", "RzR", 1.0)]
        internal static bool IsLessOrEqualWithZero(this int source)
        {
            return source <= 0;
        }
    }
}