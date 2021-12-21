using System;
using Framework.Localization.NumberWords.Enums;
using Framework.Localization.NumberWords.Interfaces;
using System.Configuration;
using System.Diagnostics;

namespace Framework.Localization.NumberWords
{
    public class StringCurrencyEvaluator
    {
        private static INumberWordsDictionary numberWordsDictionary;

        public StringCurrencyEvaluator(NumberWordsFactory factory, NumberWordsLanguages language)
        {
            numberWordsDictionary = factory.GetNumberWordsDictionary(language);
            ConfigurationSettings.GetConfig("currency-names");
        }

        public string Evaluate(double val, string curStr)
        {

            NumberWordsCurrencies currency;
            try
            {
                currency = this.GetCurrencyByStr(curStr);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
                return $"{val} {curStr}";
            }

            long n = (long)Math.Abs(val);
            long remainder = (long)((Math.Abs(val) - n + 0.005) * 100);

            NumberWordsBuilder numberWordsBuilder = new NumberWordsBuilder(numberWordsDictionary);

            if (0 == n) numberWordsBuilder.SetEmptyStringNumber();

            numberWordsBuilder.SetSeparateDecimalExponent(n, DecimalExponentsEnum.Frac);
            numberWordsBuilder.SetHignCurrency(n, currency);//Set high currency dollar, euro ....

            n /= 1000;//Less decimal exponents to thousands
            numberWordsBuilder.SetSeparateDecimalExponent(n, DecimalExponentsEnum.Thousands);

            n /= 1000;//Less decimal exponents to million
            numberWordsBuilder.SetSeparateDecimalExponent(n, DecimalExponentsEnum.Million);

            n /= 1000;//Less decimal exponents to billion
            numberWordsBuilder.SetSeparateDecimalExponent(n, DecimalExponentsEnum.Billion);

            n /= 1000;//Less decimal exponents to trillion
            numberWordsBuilder.SetSeparateDecimalExponent(n, DecimalExponentsEnum.Trillion);

            //Add the sign of value
            numberWordsBuilder.SetSign((long)val);

            numberWordsBuilder.SetRemind(remainder);
            numberWordsBuilder.SetRemindCurrency(remainder, currency);//Set currency cent, eurocent...

            return numberWordsBuilder.ResultantNumberString;
        }

        private NumberWordsCurrencies GetCurrencyByStr(string curStr)
        {
            if (string.IsNullOrEmpty(curStr))
                throw new ArgumentException("Currency is null or empty");

            if(!Enum.IsDefined(typeof(NumberWordsCurrencies), curStr.ToUpper()))
                throw new ArgumentException("The currency: " + curStr + " is not defined in dictionary");

            return (NumberWordsCurrencies)Enum.Parse(typeof(NumberWordsCurrencies), curStr);
        }
    }
}
