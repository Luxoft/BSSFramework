using System;
using System.Collections.Generic;
using System.Text;
using Framework.Localization.NumberWords.Interfaces;

namespace Framework.Localization.NumberWords.Dictionaries
{
    public class EnDecimalExponentsDictionary : IDecimalExponentsDictionary
    {
        private readonly Dictionary<DecimalExponentsEnum, string[]> dictionary = new Dictionary<DecimalExponentsEnum, string[]>
        {
            {DecimalExponentsEnum.Frac,      new [] {"",          "",          ""}},
            {DecimalExponentsEnum.Thousands, new [] {"thousand ", "thousand ", "thousands "}},
            {DecimalExponentsEnum.Million,   new [] {"million ",  "million ",  "millions "}},
            {DecimalExponentsEnum.Billion,   new [] {"billion ",  "billion ",  "billions "}},
            {DecimalExponentsEnum.Trillion,  new [] {"trillion ", "trillion ", "trillions "}}
        };

        public string[] Exponent(DecimalExponentsEnum exponents)
        {
            return this.dictionary[exponents];
        }
    }
}
