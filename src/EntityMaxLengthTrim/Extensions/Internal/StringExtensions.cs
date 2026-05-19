// ***********************************************************************
//  Assembly         : RzR.Extensions.EntityLength
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

namespace RzR.Extensions.EntityLength.Extensions.Internal
{
    /// <summary>
    ///     Local string extensions
    /// </summary>
    /// <remarks>Extension for string data type, which allows more efficient use and implement code</remarks>
    internal static class StringExtensions
    {
        /// <summary>
        ///     Check if the source string value is present
        /// </summary>
        /// <param name="source">Source string to be checked</param>
        /// <returns>Verification value, if is present > true, otherwise false</returns>
        internal static bool IsPresent(this string source)
            => !string.IsNullOrEmpty(source) && !string.IsNullOrWhiteSpace(source);

        /// <summary>
        ///     Truncates the string to a specified length and replace the truncated to a ...
        /// </summary>
        /// <param name="text">string that will be truncated</param>
        /// <param name="maxLength">total length of characters to maintain before the truncate happens</param>
        /// <param name="useDots">Use 3 dots(...) in the end of string</param>
        /// <param name="forceTrimEnd">
        ///     True to trim trailing spaces on the retained substring before appending dots;
        ///     false to preserve trailing spaces.
        /// </param>
        /// <remarks>
        ///     Base implementation from 'https://github.com/I-RzR-I/DomainCommonExtensions'
        /// </remarks>
        /// <returns>Truncated string</returns>
        internal static string Truncate(this string text, int maxLength, bool useDots = false, bool forceTrimEnd = true)
        {
            if (maxLength.IsLessOrEqualWithZero()) return text;

            const string suffix = "...";
            if (!text.IsPresent() || text.Length <= maxLength)
                return text;

            var strLength = maxLength - (useDots ? suffix.Length : 0);

            if (strLength.IsLessOrEqualWithZero())
                return useDots
                    ? suffix.Substring(0, maxLength)
                    : string.Empty;

            var truncatedString = text.Substring(0, strLength);
            if (useDots && forceTrimEnd)
                truncatedString = truncatedString.TrimEnd();

            if (useDots)
                truncatedString += suffix;

            return truncatedString;
        }


        /// <summary>
        ///     Truncates the string to a specified length and replace the truncated to a ...
        /// </summary>
        /// <param name="text">String that will be truncated</param>
        /// <param name="maxLength">Total length of characters to maintain before the truncate happens</param>
        /// <param name="useDots">Use 3 dots(...) in the start of string</param>
        /// <param name="forceTrimEnd">
        ///     Reserved for API consistency with end truncation.
        /// </param>
        /// <remarks>
        ///     Base implementation from 'https://github.com/I-RzR-I/DomainCommonExtensions'
        /// </remarks>
        /// <returns>Truncated string</returns>
        internal static string TruncateAtStart(this string text, int maxLength, bool useDots = false, bool forceTrimEnd = true)
        {
            const string prefix = "...";
            var truncatedString = text ?? string.Empty;

            if (maxLength.IsLessOrEqualWithZero())
                return truncatedString;

            if (!truncatedString.IsPresent() || truncatedString.Length <= maxLength)
                return truncatedString;

            var strLength = maxLength - (useDots ? prefix.Length : 0);

            if (strLength.IsLessOrEqualWithZero())
                return useDots
                    ? prefix.Substring(0, maxLength)
                    : string.Empty;

            truncatedString = truncatedString.Substring(truncatedString.Length - strLength, strLength);

            if (useDots)
                return prefix + truncatedString;

            return truncatedString;
        }
    }
}