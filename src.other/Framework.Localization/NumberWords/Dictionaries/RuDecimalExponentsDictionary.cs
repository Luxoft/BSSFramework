using System;
using System.Collections.Generic;
using System.Text;
using Framework.Localization.NumberWords.Interfaces;

namespace Framework.Localization.NumberWords.Dictionaries
{
    public class RuDecimalExponentsDictionary : IDecimalExponentsDictionary
    {
        private Dictionary<DecimalExponentsEnum, string[]> dictinary;

        public RuDecimalExponentsDictionary()
        {
            this.dictinary = new Dictionary<DecimalExponentsEnum, string[]>();
            this.dictinary.Add(DecimalExponentsEnum.Frac, new string[] { "", "", "" });
            this.dictinary.Add(DecimalExponentsEnum.Thousands, new string[] { "тысяча ", "тысячи ", "тысяч " });
            this.dictinary.Add(DecimalExponentsEnum.Million, new string[] { "миллион ", "миллиона ", "миллионов " });
            this.dictinary.Add(DecimalExponentsEnum.Billion, new string[] { "миллиард ", "миллиарда ", "миллиардов " });
            this.dictinary.Add(DecimalExponentsEnum.Trillion, new string[] { "триллион ", "триллиона ", "триллионов " });
        }

        public string[] Exponent(DecimalExponentsEnum exponents)
        {
            return this.dictinary[exponents];
        }
    }
}
