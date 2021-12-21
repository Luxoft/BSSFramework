using System;
using System.Collections.Generic;
using System.Text;
using Framework.Localization.NumberWords.Dictionaries;
using Framework.Localization.NumberWords.Enums;
using Framework.Localization.NumberWords.Interfaces;

namespace Framework.Localization.NumberWords
{
    public class NumberWordsBuilder
    {
        private INumberWordsDictionary numberWordsDictionary;
        private StringBuilder stringResult;

        public NumberWordsBuilder(INumberWordsDictionary numberWordsDictionary)
        {
            this.stringResult = new StringBuilder();
            this.numberWordsDictionary = numberWordsDictionary;
        }

        /// <summary>
        /// Return result of transformation number to number words
        /// </summary>
        public string ResultantNumberString
        {
            get
            {
                if (!string.IsNullOrEmpty(this.stringResult.ToString())) this.stringResult[0] = char.ToUpper(this.stringResult[0]);

                return this.stringResult.ToString();
            }
        }

        /// <summary>
        /// Function separating decimal exponents
        /// </summary>
        /// <param name="val">value that should be parsed</param>
        /// <param name="male">the male define dependency îäíà or îäèí</param>
        /// <param name="decimalExponents">define decimal exponents that shoud be determine
        /// for example thousand, million, billion...
        /// </param>
        public void SetSeparateDecimalExponent(long val, DecimalExponentsEnum exponent)
        {
            string[] decimalExponents = this.numberWordsDictionary.DecimalExponentsDictionary.Exponent(exponent);
            this.stringResult.Insert(0, this.Str(val, this.GetMale(exponent), decimalExponents[0], decimalExponents[1], decimalExponents[2]));
        }

        /// <summary>
        /// Determine currency of senior banknote. For example dollar, euro, ruble
        /// </summary>
        /// <param name="val">value that define amount of money</param>
        /// <param name="currency">currency: "RUR" - ruble, "USD" - dollar, "EUR" - euro</param>
        public void SetHignCurrency(long val, NumberWordsCurrencies currency)
        {
            if(!this.numberWordsDictionary.CurrencyInfoDictionary.Currencies.Contains(currency))
                throw new ArgumentException("The currency: " + currency + " is not registered.");

            CurrencyInfo currencyInfo = (CurrencyInfo)this.numberWordsDictionary.CurrencyInfoDictionary.Currencies[currency];
            this.stringResult.Append(this.SetCase(val,currencyInfo.seniorOne, currencyInfo.seniorTwo, currencyInfo.seniorFive));
            this.stringResult.Append(" ");
        }

        /// <summary>
        /// Set the remind of amount of money. For example amount of cents or kopecks
        /// </summary>
        /// <param name="remind">remind</param>
        public void SetRemind(long remind)
        {
            this.stringResult.Append(remind.ToString("00 "));
        }

        /// <summary>
        /// Determine currency of junior banknote . For example cent, eurocent, kopeck
        /// </summary>
        /// <param name="val">value that define amount of money</param>
        /// <param name="currency">currency: "RUR" - kopeck, "USD" - cent, "EUR" - eurocent</param>
        /// <returns></returns>
        public void SetRemindCurrency(long val, NumberWordsCurrencies currency)
        {
            if (!this.numberWordsDictionary.CurrencyInfoDictionary.Currencies.Contains(currency))
                throw new ArgumentException("The currency: " + currency + " is not registered.");

            CurrencyInfo currencyInfo = (CurrencyInfo)this.numberWordsDictionary.CurrencyInfoDictionary.Currencies[currency];

            this.stringResult.Append(this.SetCase(val, currencyInfo.juniorOne, currencyInfo.juniorTwo, currencyInfo.juniorFive));
        }

        /// <summary>
        /// Set the sign of falue
        /// </summary>
        /// <param name="val">value that define sign</param>
        public void SetSign(long val)
        {
            if (val < 0) this.stringResult.Insert(0, this.numberWordsDictionary.MathOperationDictinary.Minus);
        }

        /// <summary>
        /// Set the 0 longo result
        /// </summary>
        /// <returns></returns>
        public void SetEmptyStringNumber()
        {
            this.stringResult.Append("0 ").ToString();
        }

        private string SetCase(long val, string one, string two, string five)
        {
            long t = (val % 100 > 20) ? val % 10 : val % 20;

            switch (t)
            {
                case 1: return one;
                case 2:
                case 3:
                case 4: return two;
                default: return five;
            }
        }

        private string Str(long val, bool male, string one, string two, string five)
        {
            IDecimalNumberWordsDictionary decimalNumberWordsDictionary = this.numberWordsDictionary.DecimalNumberWordsDictionary;
            decimalNumberWordsDictionary.Male = male;

            long num = val % 1000;

            if (0 == num) return "";

            if (num < 0) throw new ArgumentOutOfRangeException(nameof(val), "Параметр не может быть отрицательным");

            StringBuilder r = new StringBuilder(decimalNumberWordsDictionary.Hunds[num / 100]);

            if (num % 100 < 20)
            {
                r.Append(decimalNumberWordsDictionary.Frac[num % 100]);
            }
            else
            {
                r.Append(decimalNumberWordsDictionary.Tens[num % 100 / 10]);
                r.Append(decimalNumberWordsDictionary.Frac[num % 10]);
            }

            r.Append(this.SetCase(num, one, two, five));

            return r.ToString();
        }

        private bool GetMale(DecimalExponentsEnum exponent)
        {
            switch (exponent)
            {
                case DecimalExponentsEnum.Thousands: return false;
                default: return true;
            }
        }
    }
}
