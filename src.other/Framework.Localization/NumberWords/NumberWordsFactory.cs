using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using Framework.Localization.NumberWords.Dictionaries;
using Framework.Localization.NumberWords.Enums;
using Framework.Localization.NumberWords.Interfaces;

namespace Framework.Localization.NumberWords
{
    public class NumberWordsFactory
    {
        private readonly Dictionary<NumberWordsLanguages, NumberWordsDictionary> dictionary = new Dictionary<NumberWordsLanguages, NumberWordsDictionary>
        {
            {
                NumberWordsLanguages.Russian,
                new NumberWordsDictionary(
                    new RuCurrencyInfoDictionary(),
                    new RuDecimalExponentsDictionary(),
                    new RuDecimalNumberWordsDictionary(),
                    new RuMathOperationWordsDictionary())
            },
            {
                NumberWordsLanguages.English,
                new NumberWordsDictionary(
                    new EnCurrencyInfoDictionary(),
                    new EnDecimalExponentsDictionary(),
                    new EnDecimalNumberWordsDictionary(),
                    new EnMathOperationWordsDictionary())
            },
            {
                NumberWordsLanguages.Ukrainian,
                new NumberWordsDictionary(
                    new UkrCurrencyInfoDictionary(),
                    new UkrDecimalExponentsDictionary(),
                    new UkrDecimalNumberWordsDictionary(),
                    new UkrMathOperationWordsDictionary())
            },
            {
                NumberWordsLanguages.Poland,
                new NumberWordsDictionary(
                    new PlCurrencyInfoDictionary(),
                    new PlDecimalExponentsDictionary(),
                    new PlDecimalNumberWordsDictionary(),
                    new PlMathOperationWordsDictionary())
            }
        };


        public INumberWordsDictionary GetNumberWordsDictionary(NumberWordsLanguages language)
        {
            NumberWordsDictionary numberWordsDictionary;
            if (!this.dictionary.TryGetValue(language, out numberWordsDictionary))
            {
                numberWordsDictionary = this.dictionary[NumberWordsLanguages.Russian];
            }

            return numberWordsDictionary;
        }
    }
}
