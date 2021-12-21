//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Configuration;
//using System.Collections.Specialized;
//using System.Xml;
//using RSDN;
//using System.Globalization;
//using System.Threading;

//namespace ENSDN
//{
//    public interface INumberWords
//    {
//        string SeparateDecimalExponent(int val, bool male, string[] decimalExponents);

//        string SetCurrency(int val, bool male,
//            string singularCurrencySize, string pluralCurrencySize, string pluralityCurrencySize);

//        string Case(int val, string singular, string plural, string plurality);

//        string Sign(int val);

//    }

//    public class EnNumberWords : INumberWords
//    {
//        private string[] enHunds =
//        {
//            "", "hundred ", "two hundred ", "three hundred ", "four hundred ",
//            "five hundred ", "six hundred ", "seven hundred ", "eight hundred ", "nine hundred "
//        };

//        private string[] enTens =
//        {
//            "", "ten ", "twenty ", "thirty ", "forty ", "fifty ",
//            "sixty ", "seventy ", "eighty ", "ninety "
//        };

//        private string[] enfrac =
//        {
//            "", "one ", "two ", "three ", "four ", "five ", "six ",
//            "seven ", "eight ", "nine ", "ten ", "eleven ",
//            "twelve ", "thrirteen ", "fourteen ", "fiveteen ",
//            "sixteen ", "seventeen ", "eighteen ", "nineteen "
//        };

//        /// <summary>
//        /// Function separating decimal exponents
//        /// </summary>
//        /// <param name="val">some int value that should be parsed</param>
//        /// <param name="male">the male define dependency одна or один</param>
//        /// <param name="decimalExponents">define decimal exponents that shoud be determine
//        /// for example thousand, million, billion...
//        /// </param>
//        /// <returns>parsed value for example 223 = двести двадцать три тыс¤чи, миллиона...</returns>
//        public string SeparateDecimalExponent(int val, bool male, string[] decimalExponents)
//        {
//            return Str(val, male, decimalExponents[0], decimalExponents[1], decimalExponents[2]);
//        }

//        /// <summary>
//        ///
//        /// </summary>
//        /// <param name="val"></param>
//        /// <param name="male"></param>
//        /// <param name="singularCurrencySize"></param>
//        /// <param name="pluralCurrencySize"></param>
//        /// <param name="pluralityCurrencySize"></param>
//        /// <returns></returns>
//        public string SetCurrency(int val, bool male,
//            string singularCurrencySize, string pluralCurrencySize, string pluralityCurrencySize)
//        {
//            return Str(val, male, singularCurrencySize, pluralCurrencySize, pluralityCurrencySize);
//        }

//        public string Str(int val, bool male, string one, string two, string five)
//        {
//            string[] frac20 = enfrac;
//            int num = val % 1000;
//            if (0 == num) return "";
//            if (num < 0) throw new ArgumentOutOfRangeException("val", "The parameter can't be less zero");
//            if (!male)
//            {
//                frac20[1] = "one ";
//                frac20[2] = "two ";
//            }

//            StringBuilder r = new StringBuilder(enHunds[num / 100]);

//            if (num % 100 < 20)
//            {
//                r.Append(frac20[num % 100]);
//            }
//            else
//            {
//                r.Append(enTens[num % 100 / 10]);
//                r.Append(frac20[num % 10]);
//            }

//            r.Append(Case(num, one, two, five));

//            if (r.Length != 0) r.Append(" ");
//            return r.ToString();
//        }

//        public string Case(int val, string singular, string plural, string plurality)
//        {
//            int t = (val % 100 > 20) ? val % 10 : val % 20;

//            switch (t)
//            {
//                case 1: return singular;
//                case 2:
//                case 3:
//                case 4: return plural;
//                default: return plurality;
//            }
//        }

//        public string Sign(int val)
//        {
//            if (val < 0)
//                return "minus ";

//            return string.Empty;
//        }
//    };

//    public class EnCurrencyInfoContainer : ICurrencyInfoContainer
//    {
//        HybridDictionary currencies = new HybridDictionary();

//        public EnCurrencyInfoContainer()
//        {
//            Register("RUR", true, "ruble", "ruble", "ruble", "kopeck", "kopeck", "kopeck");
//            Register("EUR", true, "euro", "euro", "euro", "eurocent", "eurocent", "eurocent");
//            Register("USD", true, "dollar", "dollar", "dollar", "cent", "cent", "cent");
//        }

//        private void Register(string currency, bool male,
//            string seniorOne, string seniorTwo, string seniorFive,
//            string juniorOne, string juniorTwo, string juniorFive)
//        {
//            CurrencyInfo info;
//            info.male = male;
//            info.seniorOne = seniorOne; info.seniorTwo = seniorTwo; info.seniorFive = seniorFive;
//            info.juniorOne = juniorOne; info.juniorTwo = juniorTwo; info.juniorFive = juniorFive;
//            currencies.Add(currency, info);
//        }

//        public HybridDictionary Currencies
//        {
//            get { return currencies; }
//        }
//    }

//    public class NumberWordsFactory
//    {
//        private static EnNumberWords enNumberWords;
//        private static RusNumberWords rusNumberWords;

//        private static RuDecimalExponentsContainer ruDecimalExponentsContainer;
//        private static EnDecimalExponentsContainer enDecimalExponentsContainer;

//        private static RuCurrencyInfoContainer ruCurrencyInfoContainer;
//        private static EnCurrencyInfoContainer enCurrencyInfoContainer;

//        public static INumberWords GetNumberWordsEvaluator(CultureInfo culture)
//        {
//            if (culture == CultureInfo.GetCultureInfo("ru-RU"))
//                return rusNumberWords ?? (rusNumberWords = new RusNumberWords());
//            else
//                return enNumberWords ?? (enNumberWords = new EnNumberWords());
//        }

//        public static IDecimalExponentsContainer GetDecimalExponentsContainer(CultureInfo culture)
//        {
//            if (culture == CultureInfo.GetCultureInfo("ru-RU"))
//                return ruDecimalExponentsContainer ?? (ruDecimalExponentsContainer =
//                    new RuDecimalExponentsContainer());
//            else
//                return enDecimalExponentsContainer ?? (enDecimalExponentsContainer =
//                    new EnDecimalExponentsContainer());
//        }

//        public static ICurrencyInfoContainer GetCurrencyInfoContainer(CultureInfo culture)
//        {
//            if (culture == CultureInfo.GetCultureInfo("ru-RU"))
//                return ruCurrencyInfoContainer ?? (ruCurrencyInfoContainer =
//                    new RuCurrencyInfoContainer());
//            else
//                return enCurrencyInfoContainer ?? (enCurrencyInfoContainer =
//                    new EnCurrencyInfoContainer());
//        }
//    }


//    public class EnCurrency
//    {
//        private static HybridDictionary currencies = new HybridDictionary();
//        private static CultureInfo currentCulture;

//        static EnCurrency()
//        {
//            currentCulture = Thread.CurrentThread.CurrentCulture;
//            currencies = NumberWordsFactory.GetCurrencyInfoContainer(currentCulture).Currencies;
//            ConfigurationSettings.GetConfig("currency-names");
//        }

//        public static string Str(double val)
//        {
//            return Str(val, "RUR");
//        }

//        public static string Str(double val, string currency)
//        {
//            if (!currencies.Contains(currency))
//                throw new ArgumentOutOfRangeException("currency", "Currency \"" + currency + "\" is not register");

//            CurrencyInfo info = (CurrencyInfo)currencies[currency];
//            return Str(val, info.male,
//                info.seniorOne, info.seniorTwo, info.seniorFive,
//                info.juniorOne, info.juniorTwo, info.juniorFive);
//        }

//        public static string Str(double val, bool male,
//            string seniorOne, string seniorTwo, string seniorFive,
//            string juniorOne, string juniorTwo, string juniorFive)
//        {
//            int n = val < 0 ? (int)-val : (int)val;
//            int remainder = (int)((Math.Abs(val) - n + 0.005) * 100);

//            StringBuilder r = new StringBuilder();

//            if (0 == n) r.Append("0 ");
//            if (n % 1000 != 0)
//                r.Append(NumberWordsFactory.GetNumberWordsEvaluator(currentCulture).
//                                        SetCurrency(n, male, seniorOne, seniorTwo, seniorFive));
//            else
//                r.Append(seniorFive);

//            n /= 1000;

//            r.Insert(0, NumberWordsFactory.GetNumberWordsEvaluator(currentCulture).SeparateDecimalExponent(
//                     n,
//                     false,
//                     NumberWordsFactory.GetDecimalExponentsContainer(currentCulture).Thousands));
//            n /= 1000;

//            r.Insert(0, NumberWordsFactory.GetNumberWordsEvaluator(currentCulture).SeparateDecimalExponent(
//                     n,
//                     true, NumberWordsFactory.GetDecimalExponentsContainer(currentCulture).Millions));
//            n /= 1000;

//            r.Insert(0, NumberWordsFactory.GetNumberWordsEvaluator(currentCulture).SeparateDecimalExponent(
//                     n,
//                     true,
//                     NumberWordsFactory.GetDecimalExponentsContainer(currentCulture).Billions));
//            n /= 1000;

//            r.Insert(0, NumberWordsFactory.GetNumberWordsEvaluator(currentCulture).SeparateDecimalExponent(
//                     n,
//                     true,
//                     NumberWordsFactory.GetDecimalExponentsContainer(currentCulture).Trillions));
//            n /= 1000;

//            //Add the sign of value
//            r.Insert(0, NumberWordsFactory.GetNumberWordsEvaluator(currentCulture).Sign((int)val));

//            r.Append(remainder.ToString("00 "));
//            r.Append(NumberWordsFactory.GetNumberWordsEvaluator(currentCulture).Case(remainder, juniorOne, juniorTwo, juniorFive));

//            //ƒелаем первую букву заглавной
//            r[0] = char.ToUpper(r[0]);

//            return r.ToString();
//        }
//    }
//}
