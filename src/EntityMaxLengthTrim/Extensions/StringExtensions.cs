﻿// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.EntityMaxLengthTrim
//  Author           : RzR
//  Created On       : 2022-09-24 03:47
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-01-05 09:22
// ***********************************************************************
//  <copyright file="StringExtensions.cs" company="">
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
    ///     Local string extensions
    /// </summary>
    /// <remarks>Extension for string data type, which allows more efficient use and implement code</remarks>
    internal static class StringExtensions
    {
        /// <summary>
        ///     Truncates the string to a specified length and replace the truncated to a ...
        /// </summary>
        /// <param name="text">string that will be truncated</param>
        /// <param name="maxLength">total length of characters to maintain before the truncate happens</param>
        /// <param name="useDots">Use 3 dots(...) in the end of string</param>
        /// <returns>Truncated string</returns>
        [CodeSource(
            "https://github.com/I-RzR-I/DomainCommonExtensions/blob/main/src/DomainCommonExtensions/DataTypeExtensions/StringExtensions.cs",
            "RzR", "RzR", "2022-09-27")]
        internal static string Truncate(this string text, int maxLength, bool useDots = false)
        {
            const string suffix = "...";
            var truncatedString = text;

            if (maxLength.IsLessOrEqualWithZero()) return truncatedString;
            var strLength = maxLength - (useDots.Equals(true) ? suffix.Length : 0);

            if (strLength.IsLessOrEqualWithZero()) return truncatedString;

            if (text == null || text.Length <= maxLength) return truncatedString;

            truncatedString = text.Substring(0, strLength);
            truncatedString = truncatedString.TrimEnd();

            if (useDots.Equals(true))
                truncatedString += suffix;

            return truncatedString;
        }
    }
}