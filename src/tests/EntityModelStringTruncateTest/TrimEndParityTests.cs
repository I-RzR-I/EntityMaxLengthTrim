using System;
using System.Collections.Generic;
using NUnit.Framework;
using RzR.Extensions.EntityLength;
using RzR.Extensions.EntityLength.Attributes;
using RzR.Extensions.EntityLength.Enums;
using RzR.Extensions.EntityLength.Exceptions;
using RzR.Extensions.EntityLength.Extensions;
using RzR.Extensions.EntityLength.Interceptors;
using RzR.Extensions.EntityLength.Options;

namespace EntityModelStringTruncateTest
{
    [TestFixture]
    public class TrimEndParityTests
    {
        private static readonly string Trimmed398 = new string('a', 398);
        private static readonly string OverLimit = Trimmed398 + "   ";
        private static readonly string KeptVerbatim400 = Trimmed398 + "  ";
        private static readonly string WithinLimit400 = new string('a', 397) + "   ";
        private static readonly string WithinLimit12 = new string('a', 10) + "  ";

        private static IEnumerable<TestCaseData> DefaultPaths()
        {
            yield return Path("Interceptor_Default",
                v => StringInterceptor.ApplyStringMaxAllowedLength(New(v)).Email);
            yield return Path("Interceptor_UseDots_False",
                v => StringInterceptor.ApplyStringMaxAllowedLength(New(v), false).Email);
            yield return Path("Interceptor_FourArg_ForceTrimEnd_True",
                v => StringInterceptor.ApplyStringMaxAllowedLength(New(v), false, StringTruncateType.AtTheEndOf, true).Email);
            yield return Path("Interceptor_TrimOption_Default",
                v => StringInterceptor.ApplyStringMaxAllowedLength(New(v), new TrimOption()).Email);
            yield return Path("Interceptor_StringCollection_Default",
                v => StringInterceptor.ApplyStringMaxAllowedLength(New(v), Strings()).Email);
            yield return Path("Interceptor_StringCollection_ForceTrimEnd_True",
                v => StringInterceptor.ApplyStringMaxAllowedLength(New(v), Strings(), false, StringTruncateType.AtTheEndOf, true).Email);
            yield return Path("Interceptor_PropertyName_Default",
                v => StringInterceptor.ApplyStringMaxAllowedLength(New(v), nameof(EmailModel.Email), false).Email);
            yield return Path("Interceptor_PropertyName_ForceTrimEnd_True",
                v => StringInterceptor.ApplyStringMaxAllowedLength(New(v), nameof(EmailModel.Email), false, StringTruncateType.AtTheEndOf, true).Email);
            yield return Path("Interceptor_PropertyOption_NullFlag",
                v => StringInterceptor.ApplyStringMaxAllowedLength(New(v),
                    Options(new PropertyOption { Name = nameof(EmailModel.Email) })).Email);
            yield return Path("Interceptor_PropertyOption_NoOptionForProperty",
                v => StringInterceptor.ApplyStringMaxAllowedLength(New(v), Options(), false).Email);
            yield return Path("Interceptor_ApplyProp_Default",
                v => StringInterceptor.ApplyPropStringMaxAllowedLength(New(v), nameof(EmailModel.Email), false));
            yield return Path("Fluent_Default",
                v => New(v).ToSafeStoreStrings().Email);
            yield return Path("Fluent_ForceTrimEnd_True",
                v => New(v).ToSafeStoreStrings(false, StringTruncateType.AtTheEndOf, true).Email);
            yield return Path("Fluent_StringCollection_Default",
                v => New(v).ToSafeStoreStrings(Strings()).Email);
            yield return Path("Fluent_StringCollection_ForceTrimEnd_True",
                v => New(v).ToSafeStoreStrings(Strings(), false, StringTruncateType.AtTheEndOf, true).Email);
            yield return Path("Fluent_PropertyOption_Default",
                v => New(v).ToSafeStoreStrings(Options()).Email);
            yield return Path("Fluent_PropertyOption_ForceTrimEnd_True",
                v => New(v).ToSafeStoreStrings(Options(), false, true).Email);
            yield return Path("EventBase_SetContent_AttributeLength",
                v => new AttributeLengthSetContentModel { Email = v }.Email);
            yield return Path("EventBase_SetContent_ExplicitLength",
                v => new ExplicitLengthSetContentModel { Email = v }.Email);
            yield return Path("EventBase_OnPropertyChanged",
                v => new OnPropertyChangedModel { Email = v }.Email);
        }

        private static IEnumerable<TestCaseData> OptOutPaths()
        {
            yield return Path("Interceptor_FourArg_ForceTrimEnd_False",
                v => StringInterceptor.ApplyStringMaxAllowedLength(New(v), false, StringTruncateType.AtTheEndOf, false).Email);
            yield return Path("Interceptor_TrimOption_ApplyForceTrimEnd_False",
                v => StringInterceptor.ApplyStringMaxAllowedLength(New(v), new TrimOption { ApplyForceTrimEnd = false }).Email);
            yield return Path("Interceptor_StringCollection_ForceTrimEnd_False",
                v => StringInterceptor.ApplyStringMaxAllowedLength(New(v), Strings(), false, StringTruncateType.AtTheEndOf, false).Email);
            yield return Path("Interceptor_PropertyName_ForceTrimEnd_False",
                v => StringInterceptor.ApplyStringMaxAllowedLength(New(v), nameof(EmailModel.Email), false, StringTruncateType.AtTheEndOf, false).Email);
            yield return Path("Interceptor_ApplyProp_ForceTrimEnd_False",
                v => StringInterceptor.ApplyPropStringMaxAllowedLength(New(v), nameof(EmailModel.Email), false, StringTruncateType.AtTheEndOf, false));
            yield return Path("Interceptor_PropertyOption_OptionFlag_False",
                v => StringInterceptor.ApplyStringMaxAllowedLength(New(v),
                    Options(new PropertyOption { Name = nameof(EmailModel.Email), ApplyForceTrimEnd = false })).Email);
            yield return Path("Interceptor_PropertyOption_MethodFlag_False",
                v => StringInterceptor.ApplyStringMaxAllowedLength(New(v), Options(), false, false).Email);
            yield return Path("Fluent_ForceTrimEnd_False",
                v => New(v).ToSafeStoreStrings(false, StringTruncateType.AtTheEndOf, false).Email);
            yield return Path("Fluent_StringCollection_ForceTrimEnd_False",
                v => New(v).ToSafeStoreStrings(Strings(), false, StringTruncateType.AtTheEndOf, false).Email);
            yield return Path("Fluent_PropertyOption_ForceTrimEnd_False",
                v => New(v).ToSafeStoreStrings(Options(), false, false).Email);
        }

        [TestCaseSource(nameof(DefaultPaths))]
        public void OverLimit_TrailingSpaces_DefaultPath_Trims_Trailing_Spaces(Func<string, string> path)
        {
            var result = path(OverLimit);

            AssertValueAndLength(Trimmed398, 398, result);
        }

        [Test]
        public void ApplyPropStringMaxAllowedLength_Returns_Trimmed_String_And_Leaves_Entity_Untouched()
        {
            var model = New(OverLimit);

            var result = StringInterceptor.ApplyPropStringMaxAllowedLength(model, nameof(EmailModel.Email), false);

            AssertValueAndLength(Trimmed398, 398, result);
            AssertValueAndLength(OverLimit, 401, model.Email);
        }

        [TestCaseSource(nameof(OptOutPaths))]
        public void OverLimit_TrailingSpaces_ForceTrimEndFalse_Preserves_Retained_Slice(Func<string, string> path)
        {
            var result = path(OverLimit);

            AssertValueAndLength(KeptVerbatim400, 400, result);
        }

        [Test]
        public void FourArgCall_ForceTrimEndTrue_Trims_Trailing_Spaces()
        {
            var result = StringInterceptor.ApplyStringMaxAllowedLength(
                New(OverLimit), false, StringTruncateType.AtTheEndOf, true);

            AssertValueAndLength(Trimmed398, 398, result.Email);
        }

        [Test]
        public void FourArgCall_ForceTrimEndFalse_Differs_From_True_And_Preserves_Slice()
        {
            var withTrue = StringInterceptor.ApplyStringMaxAllowedLength(
                New(OverLimit), false, StringTruncateType.AtTheEndOf, true);
            var withFalse = StringInterceptor.ApplyStringMaxAllowedLength(
                New(OverLimit), false, StringTruncateType.AtTheEndOf, false);

            AssertValueAndLength(Trimmed398, 398, withTrue.Email);
            AssertValueAndLength(KeptVerbatim400, 400, withFalse.Email);
            Assert.AreNotEqual(withTrue.Email, withFalse.Email);
        }

        [TestCaseSource(nameof(DefaultPaths))]
        public void WithinLimit_ExactLength400_TrailingSpaces_Unchanged_On_Every_Path(Func<string, string> path)
        {
            var result = path(WithinLimit400);

            AssertValueAndLength(WithinLimit400, 400, result);
        }

        [TestCaseSource(nameof(DefaultPaths))]
        public void WithinLimit_Short_TrailingSpaces_Unchanged_On_Every_Path(Func<string, string> path)
        {
            var result = path(WithinLimit12);

            AssertValueAndLength(WithinLimit12, 12, result);
        }

        [TestCaseSource(nameof(OptOutPaths))]
        public void WithinLimit_ForceTrimEndFalse_TrailingSpaces_Unchanged_On_Every_Path(Func<string, string> path)
        {
            var result = path(WithinLimit400);

            AssertValueAndLength(WithinLimit400, 400, result);
        }

        [Test]
        public void PropertyOption_FlagNull_MethodLevelTrue_Trims()
        {
            var result = ApplyWithPropertyOption(null, true);

            AssertValueAndLength(Trimmed398, 398, result);
        }

        [Test]
        public void PropertyOption_FlagNull_MethodLevelFalse_Preserves()
        {
            var result = ApplyWithPropertyOption(null, false);

            AssertValueAndLength(KeptVerbatim400, 400, result);
        }

        [Test]
        public void PropertyOption_FlagFalse_MethodLevelTrue_OptionWins_Preserves()
        {
            var result = ApplyWithPropertyOption(false, true);

            AssertValueAndLength(KeptVerbatim400, 400, result);
        }

        [Test]
        public void PropertyOption_FlagTrue_MethodLevelFalse_OptionWins_Trims()
        {
            var result = ApplyWithPropertyOption(true, false);

            AssertValueAndLength(Trimmed398, 398, result);
        }

        [Test]
        public void PropertyOption_NoOptionForProperty_MethodLevelTrue_Trims()
        {
            var result = StringInterceptor.ApplyStringMaxAllowedLength(New(OverLimit), Options(), false, true);

            AssertValueAndLength(Trimmed398, 398, result.Email);
        }

        [Test]
        public void PropertyOption_NoOptionForProperty_MethodLevelFalse_Preserves()
        {
            var result = StringInterceptor.ApplyStringMaxAllowedLength(New(OverLimit), Options(), false, false);

            AssertValueAndLength(KeptVerbatim400, 400, result.Email);
        }

        [Test]
        public void ReportOnly_Logs_Trimmed_Value_And_Leaves_Entity_Unchanged()
        {
            var logged = new List<TrimContext>();
            var model = New(OverLimit);

            StringInterceptor.ApplyStringMaxAllowedLength(model, new TrimOption
            {
                Policy = TrimPolicy.ReportOnly,
                Logger = logged.Add
            });

            Assert.AreEqual(1, logged.Count);
            Assert.AreEqual(nameof(EmailModel.Email), logged[0].PropertyName);
            AssertValueAndLength(Trimmed398, 398, logged[0].ResultValue);
            AssertValueAndLength(OverLimit, 401, logged[0].OriginalValue);
            AssertValueAndLength(OverLimit, 401, model.Email);
        }

        [Test]
        public void Warn_Logs_Trimmed_Value_And_Applies_It_To_Entity()
        {
            var logged = new List<TrimContext>();
            var model = New(OverLimit);

            StringInterceptor.ApplyStringMaxAllowedLength(model, new TrimOption
            {
                Policy = TrimPolicy.Warn,
                Logger = logged.Add
            });

            Assert.AreEqual(1, logged.Count);
            AssertValueAndLength(Trimmed398, 398, logged[0].ResultValue);
            AssertValueAndLength(Trimmed398, 398, model.Email);
        }

        [Test]
        public void Throw_Exception_Context_Carries_Original_Value_And_Entity_Is_Unchanged()
        {
            var model = New(OverLimit);

            var ex = Assert.Throws<EntityMaxLengthExceededException>(() =>
                StringInterceptor.ApplyStringMaxAllowedLength(model, new TrimOption { Policy = TrimPolicy.Throw }));

            Assert.IsNotNull(ex.Context);
            AssertValueAndLength(OverLimit, 401, ex.Context.OriginalValue);
            Assert.AreEqual(400, ex.Context.MaxLength);
            AssertValueAndLength(OverLimit, 401, model.Email);
        }

        private static TestCaseData Path(string name, Func<string, string> path)
            => new TestCaseData(path).SetArgDisplayNames(name);

        private static EmailModel New(string email) => new EmailModel { Email = email };

        private static IReadOnlyCollection<string> Strings() => new List<string>();

        private static IReadOnlyCollection<PropertyOption> Options(params PropertyOption[] options) => options;

        private static string ApplyWithPropertyOption(bool? optionFlag, bool methodLevelFlag)
        {
            var options = Options(new PropertyOption
            {
                Name = nameof(EmailModel.Email),
                ApplyForceTrimEnd = optionFlag
            });

            return StringInterceptor.ApplyStringMaxAllowedLength(New(OverLimit), options, false, methodLevelFlag).Email;
        }

        private static void AssertValueAndLength(string expected, int expectedLength, string actual)
        {
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(expectedLength, actual.Length);
        }

        private class EmailModel
        {
            [MaxAllowedLength(400)]
            public string Email { get; set; }
        }

        private class AttributeLengthSetContentModel : EntityPropChangeEventBase
        {
            private string _email;

            [MaxAllowedLength(400)]
            public string Email
            {
                get => _email;
                set => SetContent(this, nameof(Email), ref _email, ref value);
            }
        }

        private class ExplicitLengthSetContentModel : EntityPropChangeEventBase
        {
            private string _email;

            public string Email
            {
                get => _email;
                set => SetContent(this, nameof(Email), ref _email, ref value, 400);
            }
        }

        private class OnPropertyChangedModel : EntityPropChangeEventBase
        {
            private string _email;

            [MaxAllowedLength(400)]
            public string Email
            {
                get => _email;
                set
                {
                    _email = value;
                    OnPropertyChanged(this, nameof(Email));
                }
            }
        }
    }
}
