using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NUnit.Framework;
using RzR.Extensions.EntityLength.Enums;
using RzR.Extensions.EntityLength.Interceptors;
using RzR.Extensions.EntityLength.Options;

namespace EntityModelStringTruncateTest
{
    public class TrimEndEdgeCaseTests
    {
        private const string Dots = "...";

        private const string Nbsp = "\u00A0";
        private const string Zwsp = "\u200B";

        private static readonly string[] S6Inputs =
        {
            "abcdefgh   ",
            "abc   defghij",
            "   abcdefghij  ",
            "abcd\r\nefghij",
            "abcdefg\r\nxyz",
            "abc\t\t\tdefghi",
            "abcd" + Nbsp + Nbsp + "efghij",
            "ab            ",
            "abcdefghijklmnop",
            "\tabcdefgh\t\t\t",
            "\r\n\r\nabcdefgh\r\n",
            "ab" + Zwsp + "cd" + Zwsp + "efghij",
            "abcde fghi jkl ",
            "a b c d e f g h i j",
            "abcdefgh  \n  ij"
        };

        public static IEnumerable<TestCaseData> S6Cases()
        {
            foreach (var n in new[] { 4, 8 })
            for (var i = 0; i < S6Inputs.Length; i++)
            foreach (var type in new[] { StringTruncateType.AtTheEndOf, StringTruncateType.AtTheStartOf })
            foreach (var dots in new[] { false, true })
            foreach (var force in new[] { false, true })
            {
                yield return new TestCaseData(S6Inputs[i], n, type, dots, force)
                    .SetName($"S6_Input{i:00}_N{n}_{type}_Dots{dots}_Force{force}");
            }
        }

        [Test]
        public void AtTheEndOf_Crlf_CutBetweenCrAndLf_ForceTrimEndTrue_RemovesStrayCr()
        {
            var result = Apply(5, "abcd\r\nxyz", false, StringTruncateType.AtTheEndOf, true);

            Assert.AreEqual("abcd", result);
            Assert.AreEqual(4, result.Length);
        }

        [Test]
        public void AtTheEndOf_Crlf_CutBetweenCrAndLf_ForceTrimEndFalse_KeepsStrayCr()
        {
            var result = Apply(5, "abcd\r\nxyz", false, StringTruncateType.AtTheEndOf, false);

            Assert.AreEqual("abcd\r", result);
            Assert.AreEqual(5, result.Length);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void AtTheStartOf_Crlf_CutBetweenCrAndLf_KeepsLeadingLf(bool forceTrimEnd)
        {
            var result = Apply(4, "abc\r\ndef", false, StringTruncateType.AtTheStartOf, forceTrimEnd);

            Assert.AreEqual("\ndef", result);
            Assert.AreEqual(4, result.Length);
        }

        [Test]
        public void AtTheStartOf_TrailingSpaces_ForceTrimEndTrue_TrimsEndOfRetainedTail()
        {
            var result = Apply(5, "abcd  ", false, StringTruncateType.AtTheStartOf, true);

            Assert.AreEqual("bcd", result);
            Assert.AreEqual(3, result.Length);
        }

        [Test]
        public void AtTheStartOf_TrailingSpaces_ForceTrimEndFalse_KeepsRetainedTailVerbatim()
        {
            var result = Apply(5, "abcd  ", false, StringTruncateType.AtTheStartOf, false);

            Assert.AreEqual("bcd  ", result);
            Assert.AreEqual(5, result.Length);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void AtTheStartOf_LeadingSpaceOfRetainedTail_IsKept(bool forceTrimEnd)
        {
            var result = Apply(5, "abc  defg", false, StringTruncateType.AtTheStartOf, forceTrimEnd);

            Assert.AreEqual(" defg", result);
            Assert.AreEqual(5, result.Length);
        }

        [Test]
        public void AtTheEndOf_WithDots_ForceTrimEndTrue_TrimsBeforeAppendingDots()
        {
            var result = Apply(6, "ab  xyz", true, StringTruncateType.AtTheEndOf, true);

            Assert.AreEqual("ab...", result);
            Assert.AreEqual(5, result.Length);
        }

        [Test]
        public void AtTheEndOf_WithDots_ForceTrimEndFalse_KeepsSliceBeforeDots()
        {
            var result = Apply(6, "ab  xyz", true, StringTruncateType.AtTheEndOf, false);

            Assert.AreEqual("ab ...", result);
            Assert.AreEqual(6, result.Length);
        }

        [Test]
        public void AtTheStartOf_WithDots_ForceTrimEndTrue_TrimsEndOfTailAfterPrefixingDots()
        {
            var result = Apply(6, "xyz  ab  ", true, StringTruncateType.AtTheStartOf, true);

            Assert.AreEqual("...b", result);
            Assert.AreEqual(4, result.Length);
        }

        [Test]
        public void AtTheStartOf_WithDots_ForceTrimEndFalse_KeepsTailVerbatim()
        {
            var result = Apply(6, "xyz  ab  ", true, StringTruncateType.AtTheStartOf, false);

            Assert.AreEqual("...b  ", result);
            Assert.AreEqual(6, result.Length);
        }

        [TestCase(1, ".")]
        [TestCase(2, "..")]
        [TestCase(3, "...")]
        public void AtTheEndOf_TinyLimit_WithDots_ReturnsDotsPrefix_ForBothFlagValues(int maxLength, string expected)
        {
            foreach (var force in new[] { true, false })
            {
                var result = Apply(maxLength, "a b c d", true, StringTruncateType.AtTheEndOf, force);

                Assert.AreEqual(expected, result, $"forceTrimEnd={force}");
                Assert.AreEqual(maxLength, result.Length, $"forceTrimEnd={force}");
            }
        }

        [TestCase(1, ".")]
        [TestCase(2, "..")]
        [TestCase(3, "...")]
        public void AtTheStartOf_TinyLimit_WithDots_ReturnsDotsPrefix_ForBothFlagValues(int maxLength, string expected)
        {
            foreach (var force in new[] { true, false })
            {
                var result = Apply(maxLength, "a b c d", true, StringTruncateType.AtTheStartOf, force);

                Assert.AreEqual(expected, result, $"forceTrimEnd={force}");
                Assert.AreEqual(maxLength, result.Length, $"forceTrimEnd={force}");
            }
        }

        public static IEnumerable<TestCaseData> WhiteSpaceSetTrimTrueCases() => WhiteSpaceSetCases("TrimTrue");

        public static IEnumerable<TestCaseData> WhiteSpaceSetTrimFalseCases() => WhiteSpaceSetCases("TrimFalse");

        private static IEnumerable<TestCaseData> WhiteSpaceSetCases(string prefix)
        {
            yield return new TestCaseData("abcd\t\tz", "abcd", "abcd\t").SetName($"{prefix}_AC11_Tab");
            yield return new TestCaseData("abcd" + Nbsp + "z", "abcd", "abcd" + Nbsp).SetName($"{prefix}_AC11_NoBreakSpace");
            yield return new TestCaseData("abcd\nz", "abcd", "abcd\n").SetName($"{prefix}_AC11_LineFeed");
            yield return new TestCaseData("abcd" + Zwsp + "z", "abcd" + Zwsp, "abcd" + Zwsp).SetName($"{prefix}_AC11_ZeroWidthSpaceIsKept");
        }

        [TestCaseSource(nameof(WhiteSpaceSetTrimTrueCases))]
        public void AtTheEndOf_WhiteSpaceSet_ForceTrimEndTrue_TrimsOnlyCharIsWhiteSpace(
            string input, string expectedTrimmed, string expectedVerbatim)
        {
            var result = Apply(5, input, false, StringTruncateType.AtTheEndOf, true);

            Assert.AreEqual(expectedTrimmed, result);
            Assert.AreEqual(expectedTrimmed.Length, result.Length);
        }

        [TestCaseSource(nameof(WhiteSpaceSetTrimFalseCases))]
        public void AtTheEndOf_WhiteSpaceSet_ForceTrimEndFalse_KeepsSliceVerbatim(
            string input, string expectedTrimmed, string expectedVerbatim)
        {
            var result = Apply(5, input, false, StringTruncateType.AtTheEndOf, false);

            Assert.AreEqual(expectedVerbatim, result);
            Assert.AreEqual(5, result.Length);
        }

        [Test]
        public void AtTheEndOf_SliceIsWhiteSpaceOnly_NoDots_ForceTrimEndTrue_ReturnsEmpty()
        {
            var result = Apply(3, "   abc", false, StringTruncateType.AtTheEndOf, true);

            Assert.AreEqual(string.Empty, result);
            Assert.AreEqual(0, result.Length);
        }

        [Test]
        public void AtTheEndOf_SliceIsWhiteSpaceOnly_WithDots_ForceTrimEndTrue_ReturnsDotsOnly()
        {
            var result = Apply(6, "      abc", true, StringTruncateType.AtTheEndOf, true);

            Assert.AreEqual(Dots, result);
            Assert.AreEqual(3, result.Length);
        }

        [Test]
        public void AtTheStartOf_SliceIsWhiteSpaceOnly_NoDots_ForceTrimEndTrue_ReturnsEmpty()
        {
            var result = Apply(3, "abc   ", false, StringTruncateType.AtTheStartOf, true);

            Assert.AreEqual(string.Empty, result);
            Assert.AreEqual(0, result.Length);
        }

        [Test]
        public void AtTheStartOf_SliceIsWhiteSpaceOnly_WithDots_ForceTrimEndTrue_ReturnsDotsOnly()
        {
            var result = Apply(6, "abc      ", true, StringTruncateType.AtTheStartOf, true);

            Assert.AreEqual(Dots, result);
            Assert.AreEqual(3, result.Length);
        }

        [Test]
        public void AtTheEndOf_SliceIsWhiteSpaceOnly_NoDots_ForceTrimEndFalse_KeepsWhiteSpaceSlice()
        {
            var result = Apply(3, "   abc", false, StringTruncateType.AtTheEndOf, false);

            Assert.AreEqual("   ", result);
            Assert.AreEqual(3, result.Length);
        }

        [Test]
        public void AtTheStartOf_SliceIsWhiteSpaceOnly_WithDots_ForceTrimEndFalse_KeepsWhiteSpaceSlice()
        {
            var result = Apply(6, "abc      ", true, StringTruncateType.AtTheStartOf, false);

            Assert.AreEqual("...   ", result);
            Assert.AreEqual(6, result.Length);
        }

        public static IEnumerable<TestCaseData> UnchangedCases()
        {
            var values = new[]
            {
                new object[] { "ab   ", 5 },
                new object[] { "ab  ", 5 },
                new object[] { "ab\r\n", 5 },
                new object[] { "      ", 3 },
                new object[] { " \t\r\n " + Nbsp + " ", 3 }
            };

            foreach (var v in values)
            foreach (var type in new[] { StringTruncateType.AtTheEndOf, StringTruncateType.AtTheStartOf })
            foreach (var dots in new[] { false, true })
            foreach (var force in new[] { false, true })
            {
                var input = (string)v[0];
                var n = (int)v[1];
                yield return new TestCaseData(input, n, type, dots, force)
                    .SetName($"Unchanged_Len{input.Length}_N{n}_{type}_Dots{dots}_Force{force}_{Describe(input)}");
            }
        }

        [TestCaseSource(nameof(UnchangedCases))]
        public void WithinLimit_Or_WhiteSpaceOnly_Value_IsReturnedUnchanged(
            string input, int n, StringTruncateType type, bool dots, bool force)
        {
            var result = Apply(n, input, dots, type, force);

            Assert.AreEqual(input, result);
            Assert.AreEqual(input.Length, result.Length);
        }

        [TestCaseSource(nameof(S6Cases))]
        public void OverLength_Result_NeverExceedsLimit_And_RetainedPartFollowsFlag(
            string input, int n, StringTruncateType type, bool dots, bool force)
        {
            Assert.Greater(input.Length, n, "precondition: fixed inputs must be over-length");
            var keep = dots ? n - Dots.Length : n;
            var rawSlice = type == StringTruncateType.AtTheEndOf
                ? input.Substring(0, keep)
                : input.Substring(input.Length - keep, keep);

            var result = Apply(n, input, dots, type, force);

            Assert.LessOrEqual(result.Length, n);

            string retained;
            if (!dots)
            {
                retained = result;
            }
            else if (type == StringTruncateType.AtTheEndOf)
            {
                Assert.IsTrue(result.EndsWith(Dots, StringComparison.Ordinal), "dots suffix expected");
                retained = result.Substring(0, result.Length - Dots.Length);
            }
            else
            {
                Assert.IsTrue(result.StartsWith(Dots, StringComparison.Ordinal), "dots prefix expected");
                retained = result.Substring(Dots.Length);
            }

            if (force)
            {
                Assert.AreEqual(rawSlice.TrimEnd(), retained);
                if (retained.Length > 0)
                    Assert.IsFalse(char.IsWhiteSpace(retained[retained.Length - 1]),
                        "retained part must not end with white-space");
            }
            else
            {
                Assert.AreEqual(rawSlice, retained);
            }
        }

        private static string Describe(string value)
            => value.Replace("\r", "CR").Replace("\n", "LF").Replace("\t", "TAB")
                .Replace(Nbsp, "NBSP").Replace(" ", "_");

        private static string Apply(int maxLength, string value, bool useDots, StringTruncateType type, bool force)
        {
            var option = new TrimOption
            {
                UseDots = useDots,
                TruncateType = type,
                ApplyForceTrimEnd = force
            };

            switch (maxLength)
            {
                case 1: return StringInterceptor.ApplyStringMaxAllowedLength(new Max1Model { Value = value }, option).Value;
                case 2: return StringInterceptor.ApplyStringMaxAllowedLength(new Max2Model { Value = value }, option).Value;
                case 3: return StringInterceptor.ApplyStringMaxAllowedLength(new Max3Model { Value = value }, option).Value;
                case 4: return StringInterceptor.ApplyStringMaxAllowedLength(new Max4Model { Value = value }, option).Value;
                case 5: return StringInterceptor.ApplyStringMaxAllowedLength(new Max5Model { Value = value }, option).Value;
                case 6: return StringInterceptor.ApplyStringMaxAllowedLength(new Max6Model { Value = value }, option).Value;
                case 8: return StringInterceptor.ApplyStringMaxAllowedLength(new Max8Model { Value = value }, option).Value;
                default:
                    throw new ArgumentOutOfRangeException(nameof(maxLength), maxLength, "No model for this limit.");
            }
        }

        private class Max1Model
        {
            [MaxLength(1)]
            public string Value { get; set; }
        }

        private class Max2Model
        {
            [MaxLength(2)]
            public string Value { get; set; }
        }

        private class Max3Model
        {
            [MaxLength(3)]
            public string Value { get; set; }
        }

        private class Max4Model
        {
            [MaxLength(4)]
            public string Value { get; set; }
        }

        private class Max5Model
        {
            [MaxLength(5)]
            public string Value { get; set; }
        }

        private class Max6Model
        {
            [MaxLength(6)]
            public string Value { get; set; }
        }

        private class Max8Model
        {
            [MaxLength(8)]
            public string Value { get; set; }
        }
    }
}
