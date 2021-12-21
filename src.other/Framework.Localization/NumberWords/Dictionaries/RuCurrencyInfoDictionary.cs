using Framework.Localization.NumberWords.Enums;
using Framework.Localization.NumberWords.Interfaces;
using System.Collections.Specialized;

namespace Framework.Localization.NumberWords.Dictionaries
{
    public class RuCurrencyInfoDictionary : ICurrencyInfoDictionary
    {
        HybridDictionary currencies = new HybridDictionary();

        public RuCurrencyInfoDictionary()
        {
            this.Register(NumberWordsCurrencies.RUR, true, "рубль", "рубля", "рублей", "копейка", "копейки", "копеек");
            this.Register(NumberWordsCurrencies.EUR, true, "евро", "евро", "евро", "евроцент", "евроцента", "евроцентов");
            this.Register(NumberWordsCurrencies.USD, true, "доллар", "доллара", "долларов", "цент", "цента", "центов");
            this.Register(NumberWordsCurrencies.GBP, true, "фунт", "фунта", "фунтов", "пенс", "пенса", "пенсов");
            this.Register(NumberWordsCurrencies.CHF, true, "франк", "франка", "франков", "сантим", "сантима", "сантимов");
            this.Register(NumberWordsCurrencies.CAD, true, "доллар", "доллара", "долларов", "цент", "цента", "центов");
            this.Register(NumberWordsCurrencies.UAH, false, "гривна", "гривны", "гривен", "копейка", "копейки", "копеек");
            this.Register(NumberWordsCurrencies.RON, true, "лей", "лея", "леев", "бан", "бана", "банов");
            this.Register(NumberWordsCurrencies.JPY, true, "иена", "иены", "иен", "сена", "сены", "сен");
            this.Register(NumberWordsCurrencies.PLN, true, "злотый", "злотых", "злотых", "грош", "гроша", "грошей");

            this.Register(NumberWordsCurrencies.RUB, true, "рубль", "рубля", "рублей", "копейка", "копейки", "копеек");
            this.Register(NumberWordsCurrencies.VND, true, "донг", "донга", "донгов", "су", "су", "су");
            this.Register(NumberWordsCurrencies.INR, true, "рупия", "рупии", "рупий", "пайс", "пайса", "пайсов");
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
