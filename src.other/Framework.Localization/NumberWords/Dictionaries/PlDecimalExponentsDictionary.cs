using System.Collections.Generic;
using Framework.Localization.NumberWords.Interfaces;
using Framework.Localization.NumberWords;

namespace Framework.Localization.NumberWords.Dictionaries
{
    public class PlDecimalExponentsDictionary: IDecimalExponentsDictionary
    {
        private Dictionary<DecimalExponentsEnum, string[]> dictinary;

        public PlDecimalExponentsDictionary() {
            this.dictinary=new Dictionary<DecimalExponentsEnum, string[]>();
            this.dictinary.Add(DecimalExponentsEnum.Frac, new string[] { "", "", "" });
            this.dictinary.Add(DecimalExponentsEnum.Thousands, new string[] { "tysiąc ", "tysiące ", "tysięcy " });
            this.dictinary.Add(DecimalExponentsEnum.Million, new string[] { "milion ", "miliony ", "milionów " });
            this.dictinary.Add(DecimalExponentsEnum.Billion, new string[] { "miliard ", "miliardy ", "miliardów " });
            this.dictinary.Add(DecimalExponentsEnum.Trillion, new string[] { "bilion ", "biliony ", "bilionów " });
        }

        public string[] Exponent(DecimalExponentsEnum exponents) {
        return this.dictinary[exponents];
        }
    }
}