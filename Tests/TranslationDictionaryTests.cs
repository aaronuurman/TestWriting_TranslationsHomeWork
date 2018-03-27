using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestWriting_TranslationsHomeWork;

namespace Tests
{
    [TestClass]
    public class TranslationDictionaryTests
    {
        public TranslationDictionaryTests()
        {
            _translationDictionary = new TranslationDictionary();
        }

        private readonly TranslationDictionary _translationDictionary;

        /// <summary>
        /// The issue is described as following:
        ///     The tests involve quite a lot of code, which starts to get into maintainability issues.
        ///     I find that in such cases it is useful to define some shared variables in the class for commonly used test data.
        /// 
        /// To reduce code duplication in tests added these static values.
        /// </summary>
        private static readonly Translation EnglishTranslation = new Translation()
        {
            FromWord = "siin",
            FromLanguage = "Estonian",
            ToWord = "here",
            ToLanguage = "English"
        };

        private static readonly Translation RussianTranslation = new Translation()
        {
            FromWord = "siin",
            FromLanguage = "Estonian",
            ToWord = "здесь",
            ToLanguage = "Russian"
        };

        /// Also added Chinese and Arabic to simulate different cultures.
        private static readonly Translation ChineseTranslation = new Translation()
        {
            FromWord = "siin",
            FromLanguage = "Estonian",
            ToWord = "这里",
            ToLanguage = "Chinese"
        };

        private static readonly Translation ArabicTranslation = new Translation()
        {
            FromWord = "siin",
            FromLanguage = "Estonian",
            ToWord = "هنا",
            ToLanguage = "Arabic"
        };

        /// <summary>
        /// Creating this kind of reusable test data helps me cover all critical cultures easily and without code duplication.
        /// </summary>
        private static IEnumerable<object[]> ReusableTestData =>
            new List<object[]>
            {
                new object[]{ EnglishTranslation.FromWord, EnglishTranslation.FromLanguage, EnglishTranslation.ToWord, EnglishTranslation.ToLanguage },
                new object[]{ RussianTranslation.FromWord, RussianTranslation.FromLanguage, RussianTranslation.ToWord, RussianTranslation.ToLanguage },
                new object[]{ ChineseTranslation.FromWord, ChineseTranslation.FromLanguage, ChineseTranslation.ToWord, ChineseTranslation.ToLanguage },
                new object[]{ ArabicTranslation.FromWord, ArabicTranslation.FromLanguage, ArabicTranslation.ToWord, ArabicTranslation.ToLanguage},
            };

        [TestMethod]
        public void NewDictionary_HasZeroTanslations()
        {
            Assert.AreEqual(0, _translationDictionary.Translations.Count);
        }

        [TestMethod]
        [DynamicData(nameof(ReusableTestData))]
        public void AddEntry_WorksWith_ValidInput(string fromWord, string fromLanguage, string toWord, string toLanguage)
        {
            _translationDictionary.AddEntry(fromWord, fromLanguage, toWord, toLanguage);
            Assert.AreEqual(1, _translationDictionary.Translations.Count);
        }

        /// <summary>
        /// The issue is described as following:
        /// “Null as input throws exception” style tests are very low value – I would not even create such tests.
        /// 
        /// Removed "null as input" tests.
        /// </summary>
        [TestMethod]
        public void AddEntry_WithInvalidInput_ThrowsException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _translationDictionary.AddEntry("", "Estonian", "here", "English"));
            Assert.ThrowsException<ArgumentNullException>(() => _translationDictionary.AddEntry("siin", " ", "here", "English"));
            Assert.ThrowsException<ArgumentNullException>(() => _translationDictionary.AddEntry("siin", "Estonian", "", "English"));
            Assert.ThrowsException<ArgumentNullException>(() => _translationDictionary.AddEntry("siin", "Estonian", "here", " "));
        }

        [TestMethod]
        [DynamicData(nameof(ReusableTestData))]
        public void Remove_WorksWith_ValidInput(string fromWord, string fromLanguage, string toWord, string toLanguage)
        {
            var t = new Translation()
            {
                FromWord = fromWord,
                FromLanguage = fromLanguage,
                ToWord = toWord,
                ToLanguage = toLanguage
            };

            _translationDictionary.Translations.Add(t);

            _translationDictionary.Remove(t.ToWord, t.ToLanguage);

            Assert.AreEqual(0, _translationDictionary.Translations.Count);
        }

        /// <summary>
        /// Reused previously created data.
        /// </summary>
        [TestMethod]
        public void Remove_CaseInsensitive_WorksWith_ValidInput()
        {
            _translationDictionary.Translations.Add(EnglishTranslation);

            _translationDictionary.Remove("SIIN", "EsToNiAn");

            Assert.AreEqual(0, _translationDictionary.Translations.Count);
        }

        [TestMethod]
        public void Remove_InvalidInput_ThrowsException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _translationDictionary.Remove("", "English"));
            Assert.ThrowsException<ArgumentNullException>(() => _translationDictionary.Remove("here", " "));
            Assert.ThrowsException<Exception>(() => _translationDictionary.Remove("siin", "English"));
        }

        /// <summary>
        /// Reused previously created data.
        /// </summary>
        [TestMethod]
        public void Clear_Works()
        {
            var t = EnglishTranslation;
            _translationDictionary.Translations.Add(t);

            _translationDictionary.Clear();

            Assert.AreEqual(0, _translationDictionary.Translations.Count);
        }

        /// <summary>
        /// Reused previously created data.
        /// </summary>
        [TestMethod]
        //Todo: Now when I can use DynamicData, gave me idea to add reverse translation functionality into project.
        public void Translate_WorksWith_ValidInput()
        {
            var l = new List<Translation>()
            {
                EnglishTranslation,
                RussianTranslation,
                ChineseTranslation,
                ArabicTranslation
            };

            _translationDictionary.Translations.AddRange(l);

            var result = _translationDictionary.Translate("siin", "Estonian", "Russian");

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("здесь", result[0].ToWord);
        }

        /// <summary>
        /// Reused previously created data.
        /// </summary>
        [TestMethod]
        public void Translate_WorksWith_MultipleResults()
        {
            var l = new List<Translation>()
            {
                EnglishTranslation,
                RussianTranslation,
                ChineseTranslation,
                ArabicTranslation,
                new Translation()
                {
                    FromWord = "siin",
                    FromLanguage = "Estonian",
                    ToWord = "здесь 2",
                    ToLanguage = "Russian"
                }
            };

            _translationDictionary.Translations.AddRange(l);

            var result = _translationDictionary.Translate("siin", "Estonian", "Russian");

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("здесь", result[0].ToWord);
            Assert.AreEqual("здесь 2", result[1].ToWord);
        }

        /// <summary>
        /// The issue is described as following:
        /// “Null as input throws exception” style tests are very low value – I would not even create such tests.
        /// 
        /// Removed "null as input" tests.
        /// </summary>
        [TestMethod]
        public void Translate_InvalidInput_ThrowsException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _translationDictionary.Translate("", "Estonian", "English"));
            Assert.ThrowsException<ArgumentNullException>(() => _translationDictionary.Translate("siin", " ", "English"));
            Assert.ThrowsException<ArgumentNullException>(() => _translationDictionary.Translate("siin", "Estonian", " "));
        }

        /// <summary>
        /// Reused previously created data.
        /// </summary>
        [TestMethod]
        public void Translate_UntranslatedLanguage_ReturnsEmptyList()
        {
            _translationDictionary.Translations.AddRange(new List<Translation>()
            {
                EnglishTranslation,
                RussianTranslation,
                ChineseTranslation,
                ArabicTranslation
            });

            var result = _translationDictionary.Translate("siin", "Estonian", "Latvian");
            Assert.AreEqual(0, result.Count);
        }
    }
}
