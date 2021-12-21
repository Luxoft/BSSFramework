using Framework.Localization.NumberWords.Enums;
using Framework.Localization.NumberWords.Interfaces;
using System.Collections.Specialized;

namespace Framework.Localization.NumberWords.Dictionaries
{
    public class EnCurrencyInfoDictionary : ICurrencyInfoDictionary
    {
        HybridDictionary currencies = new HybridDictionary();

        public EnCurrencyInfoDictionary()
        {
            this.Register(NumberWordsCurrencies.RUR, true, "ruble", "rubles", "rubles", "kopeck", "kopecks", "kopecks");
            this.Register(NumberWordsCurrencies.EUR, true, "euro", "euro", "euro", "eurocent", "eurocents", "eurocents");
            this.Register(NumberWordsCurrencies.USD, true, "dollar", "dollars", "dollars", "cent", "cents", "cents");
            this.Register(NumberWordsCurrencies.GBP, true, "pound", "pounds", "pounds", "penny", "pence", "pence");
            this.Register(NumberWordsCurrencies.CHF, true, "franc", "francs", "francs", "centime", "centimes", "centimes");
            this.Register(NumberWordsCurrencies.CAD, true, "dollar", "dollars", "dollars", "cent", "cents", "cents");
            this.Register(NumberWordsCurrencies.UAH, false, "hryvna", "hryvni", "hryven’", "kopiyka", "kopiyky", "kopiyok");
            this.Register(NumberWordsCurrencies.RON, true, "leu", "lei", "lei", "ban", "bani", "bani");
            this.Register(NumberWordsCurrencies.JPY, true, "jen", "jens", "jens", "sen", "sens", "sens");
            this.Register(NumberWordsCurrencies.PLN, true, "zloty", "zloties", "zloties", "groszy", "groszies", "groszies");

            this.Register(NumberWordsCurrencies.RUB, true, "ruble", "rubles", "rubles", "kopeck", "kopecks", "kopecks");
            this.Register(NumberWordsCurrencies.VND, true, "dong", "dongs", "dongs", "xu", "xus", "xus");
            this.Register(NumberWordsCurrencies.INR, true, "rupee", "rupees", "rupees", "paisa", "paise", "paise");
        }

        private void Register(NumberWordsCurrencies currency, bool male,
            string seniorOne, string seniorTwo, string seniorFive,
            string juniorOne, string juniorTwo, string juniorFive)
        {
            CurrencyInfo info;
            info.male = male;
            info.seniorOne = seniorOne; info.seniorTwo = seniorTwo; info.seniorFive = seniorFive;
            info.juniorOne = juniorOne; info.juniorTwo = juniorTwo; info.juniorFive = juniorFive;
            this.currencies.Add(currency, info);
        }

        public HybridDictionary Currencies
        {
            get { return this.currencies; }
        }
    }
}
