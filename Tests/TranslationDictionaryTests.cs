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

        [TestMethod]
        public void NewDictionary_HasZeroTanslations()
        {
            Assert.AreEqual(0, _translationDictionary.Translations.Count);
        }

        [TestMethod]
        public void AddEntry_WorksWith_ValidInput()
        {
            var t = new Translation()
            {
                FromWord = "siin",
                FromLanguage = "Estonian",
                ToWord = "here",
                ToLanguage = "English"
            };
            _translationDictionary.AddEntry(t.FromWord, t.FromLanguage, t.ToWord, t.ToLanguage);
            Assert.AreEqual(1, _translationDictionary.Translations.Count);
        }

        [TestMethod]
        public void AddEntry_WithInvalidInput_ThrowsException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _translationDictionary.AddEntry("", "Estonian", "here", "English"));
            Assert.ThrowsException<ArgumentNullException>(() => _translationDictionary.AddEntry("siin", " ", "here", "English"));
            Assert.ThrowsException<ArgumentNullException>(() => _translationDictionary.AddEntry("siin", "Estonian", null, "English"));
            Assert.ThrowsException<ArgumentNullException>(() => _translationDictionary.AddEntry("siin", "Estonian", "here", null));
        }

        [TestMethod]
        public void Remove_WorksWith_ValidInput()
        {
            var t = new Translation()
            {
                FromWord = "siin",
                FromLanguage = "Estonian",
                ToWord = "here",
                ToLanguage = "English"
            };
            _translationDictionary.Translations.Add(t);

            _translationDictionary.Remove(t.ToWord, t.ToLanguage);
            
            Assert.AreEqual(0, _translationDictionary.Translations.Count);
        }

        [TestMethod]
        public void Remove_CaseInsensitive_WorksWith_ValidInput()
        {
            var t = new Translation()
            {
                FromWord = "siin",
                FromLanguage = "Estonian",
                ToWord = "here",
                ToLanguage = "English"
            };
            _translationDictionary.Translations.Add(t);

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

        [TestMethod]
        public void Clear_Works()
        {
            var t = new Translation()
            {
                FromWord = "siin",
                FromLanguage = "Estonian",
                ToWord = "here",
                ToLanguage = "English"
            };
            _translationDictionary.Translations.Add(t);

            _translationDictionary.Clear();

            Assert.AreEqual(0, _translationDictionary.Translations.Count);
        }

        [TestMethod]
        public void Translate_WorksWith_ValidInput()
        {
            var l = new List<Translation>()
            {
                new Translation()
                {
                    FromWord = "siin",
                    FromLanguage = "Estonian",
                    ToWord = "here",
                    ToLanguage = "English"
                },
                new Translation()
                {
                    FromWord = "siin",
                    FromLanguage = "Estonian",
                    ToWord = "здесь",
                    ToLanguage = "Russian"
                }
            };

            _translationDictionary.Translations.AddRange(l);

            var result = _translationDictionary.Translate("siin", "Estonian", "Russian");
            
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("здесь", result[0].ToWord);
        }

        [TestMethod]
        public void Translate_WorksWith_MultipleResults()
        {
            var l = new List<Translation>()
            {
                new Translation()
                {
                    FromWord = "siin",
                    FromLanguage = "Estonian",
                    ToWord = "here",
                    ToLanguage = "English"
                },
                new Translation()
                {
                    FromWord = "siin",
                    FromLanguage = "Estonian",
                    ToWord = "здесь",
                    ToLanguage = "Russian"
                },
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

        [TestMethod]
        public void Translate_InvalidInput_ThrowsException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _translationDictionary.Translate("", "Estonian", "English"));
            Assert.ThrowsException<ArgumentNullException>(() => _translationDictionary.Translate("siin", " ", "English"));
            Assert.ThrowsException<ArgumentNullException>(() => _translationDictionary.Translate("siin", "Estonian", null));
        }

        [TestMethod]
        public void Translate_UntranslatedLanguage_ReturnsEmptyList()
        {
            var l = new List<Translation>()
            {
                new Translation()
                {
                    FromWord = "siin",
                    FromLanguage = "Estonian",
                    ToWord = "here",
                    ToLanguage = "English"
                },
                new Translation()
                {
                    FromWord = "siin",
                    FromLanguage = "Estonian",
                    ToWord = "здесь",
                    ToLanguage = "Russian"
                }
            };

            _translationDictionary.Translations.AddRange(l);

            var result = _translationDictionary.Translate("siin", "Estonian", "Latvian");
            Assert.AreEqual(0, result.Count);
        }
    }
}
