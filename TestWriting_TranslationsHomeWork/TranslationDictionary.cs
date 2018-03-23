using System;
using System.Collections.Generic;

namespace TestWriting_TranslationsHomeWork
{
    public class TranslationDictionary
    {
        public TranslationDictionary()
        {
            Translations = new List<Translation>();
        }

        public List<Translation> Translations;

        /// <summary>
        /// Add new Translations.
        /// </summary>
        public void AddEntry(string fromWord, string fromLanguage, string toWord, string toLanguage)
        {
            if (string.IsNullOrWhiteSpace(fromWord))
                throw new ArgumentNullException(nameof(fromWord), "Please define fromWord.");
            
            if (string.IsNullOrWhiteSpace(fromLanguage))
                throw new ArgumentNullException(nameof(fromLanguage), "Please define fromLanguage.");

            if (string.IsNullOrWhiteSpace(toWord))
                throw new ArgumentNullException(nameof(toWord), "Please define toWord.");

            if (string.IsNullOrWhiteSpace(toLanguage))
                throw new ArgumentNullException(nameof(toLanguage), "Please define toLanguage.");

            Translations.Add(new Translation()
            {
                FromWord = fromWord,
                FromLanguage = fromLanguage,
                ToWord = toWord,
                ToLanguage = toLanguage
            });
        }

        /// <summary>
        /// Remove translation. 
        /// </summary>
        public void Remove(string word, string language)
        {
            if (string.IsNullOrWhiteSpace(word))
                throw new ArgumentNullException(nameof(word), "Please define word that to remove.");

            if (string.IsNullOrWhiteSpace(language))
                throw new ArgumentNullException(nameof(language), "Please define language.");

            var t = Translations.Find(x =>
                        (x.FromWord.Equals(word, StringComparison.InvariantCultureIgnoreCase) &&
                         x.FromLanguage.Equals(language, StringComparison.InvariantCultureIgnoreCase))) ?? 
                    Translations.Find(x =>
                        (x.ToWord.Equals(word, StringComparison.InvariantCultureIgnoreCase) &&
                         x.ToLanguage.Equals(language, StringComparison.InvariantCultureIgnoreCase)));

            if (t != null)
                Translations.Remove(t);
            else
                throw new Exception("Translation was not found");
        }

        /// <summary>
        /// Remove all translations from dictionary.
        /// </summary>
        public void Clear()
        {
            Translations.Clear();
        }

        /// <summary>
        /// Translate word.
        /// </summary>
        public List<Translation> Translate(string fromWord, string fromLanguage, string toLanguage)
        {
            if (string.IsNullOrWhiteSpace(fromWord))
                throw new ArgumentNullException(nameof(fromWord), "Please define fromWord.");

            if (string.IsNullOrWhiteSpace(fromLanguage))
                throw new ArgumentNullException(nameof(fromLanguage), "Please define fromLanguage.");

            if (string.IsNullOrWhiteSpace(toLanguage))
                throw new ArgumentNullException(nameof(toLanguage), "Please define toLanguage.");

            var words = Translations.FindAll(x =>
                x.FromWord.Equals(fromWord, StringComparison.InvariantCultureIgnoreCase) &&
                x.FromLanguage.Equals(fromLanguage, StringComparison.InvariantCultureIgnoreCase) &&
                x.ToLanguage.Equals(toLanguage, StringComparison.InvariantCultureIgnoreCase));
            return words;
        }
    }
}
