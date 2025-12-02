// ***********************************************************************
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

namespace EntityMaxLengthTrim.Extensions.Internal
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
        /// <returns>Truncated string</returns>
        [CodeSource("https://github.com/I-RzR-I/DomainCommonExtensions", "RzR", "RzR", "2022-09-27")]
        internal static string Truncate(this string text, int maxLength, bool useDots = false)
        {
            if (maxLength.IsLessOrEqualWithZero()) return text;

            const string suffix = "...";
            var truncatedString = text;

            var strLength = maxLength - (useDots.Equals(true) ? suffix.Length : 0);

            if (strLength.IsLessOrEqualWithZero()) return truncatedString;
            if (!text.IsPresent() || text.Length <= maxLength) return truncatedString;

            truncatedString = text.Substring(0, strLength);
            truncatedString = truncatedString.TrimEnd();

            if (useDots.Equals(true))
                truncatedString += suffix;

            return truncatedString;
        }


        /// <summary>
        ///     Truncates the string to a specified length and replace the truncated to a ...
        /// </summary>
        /// <param name="text">String that will be truncated</param>
        /// <param name="maxLength">Total length of characters to maintain before the truncate happens</param>
        /// <param name="useDots">Use 3 dots(...) in the start of string</param>
        /// <returns>Truncated string</returns>
        [CodeSource("https://github.com/I-RzR-I/DomainCommonExtensions", "RzR", "RzR", "2025-08-25")]
        internal static string TruncateAtStart(this string text, int maxLength, bool useDots = false)
        {
            const string prefix = "...";
            var truncatedString = text ?? string.Empty;

            if (maxLength.IsLessOrEqualWithZero()) return truncatedString;
            var strLength = maxLength - (useDots.Equals(true) ? prefix.Length : 0);

            if (strLength.IsLessOrEqualWithZero()) return truncatedString;

            if (!text.IsPresent() || text!.Length <= maxLength) return truncatedString;

            truncatedString = text.Substring(text.Length - strLength, strLength);
            truncatedString = truncatedString.TrimEnd();

            if (useDots.Equals(true))
                return prefix + truncatedString;

            return truncatedString;
        }
    }
}