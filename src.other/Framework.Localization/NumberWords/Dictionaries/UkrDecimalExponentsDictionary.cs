using System.Collections.Generic;
using Framework.Localization.NumberWords.Interfaces;

namespace Framework.Localization.NumberWords.Dictionaries
{
    public class UkrDecimalExponentsDictionary : IDecimalExponentsDictionary
    {
        private Dictionary<DecimalExponentsEnum, string[]> dictinary;

        public UkrDecimalExponentsDictionary()
        {
            this.dictinary = new Dictionary<DecimalExponentsEnum, string[]>();
            this.dictinary.Add(DecimalExponentsEnum.Frac, new string[] { "", "", "" });
            this.dictinary.Add(DecimalExponentsEnum.Thousands, new string[] { "тисяча ", "тисячi ", "тисяч " });
            this.dictinary.Add(DecimalExponentsEnum.Million, new string[] { "мiльйон ", "мiльйони ", "мiльйонiв " });
            this.dictinary.Add(DecimalExponentsEnum.Billion, new string[] { "мільярд ", "мільярди ", "мільярдiв " });
            this.dictinary.Add(DecimalExponentsEnum.Trillion, new string[] { "трильйон ", "трильйони ", "трильйонів " });
        }

        public string[] Exponent(DecimalExponentsEnum exponents)
        {
            return this.dictinary[exponents];
        }
    }
}