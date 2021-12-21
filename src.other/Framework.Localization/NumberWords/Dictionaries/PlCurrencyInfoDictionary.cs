using Framework.Localization.NumberWords.Interfaces;
using System.Collections.Specialized;
using Framework.Localization.NumberWords.Enums;

namespace Framework.Localization.NumberWords.Dictionaries
{
    public class PlCurrencyInfoDictionary: ICurrencyInfoDictionary
    {
        HybridDictionary currencies=new HybridDictionary();

        public PlCurrencyInfoDictionary()
        {
            this.Register(NumberWordsCurrencies.PLN, true, "złoty", "złote", "złotych", "grosz", "grosze", "groszy");
            this.Register(NumberWordsCurrencies.RUR, true, "rubel", "ruble", "rubli", "kopiejka", "kopiejki", "kopiejek");
            this.Register(NumberWordsCurrencies.EUR, true, "euro", "euro", "euro", "eurocent", "eurocenty", "eurocentów");
            this.Register(NumberWordsCurrencies.USD, true, "dolar", "dolary", "dolarów", "cent", "centy", "centów");
            this.Register(NumberWordsCurrencies.GBP, true, "funt", "funty", "funtów", "пенс", "пенса", "пенсов");
            this.Register(NumberWordsCurrencies.CHF, true, "frank", "franki", "franków", "centym", "centymy", "centymów");
            this.Register(NumberWordsCurrencies.CAD, true, "dolar", "dolary", "dolarów", "cent", "centy", "centów");
            this.Register(NumberWordsCurrencies.UAH, false, "hrywna", "hrywny", "hrywen", "kopiejka", "kopiejki", "kopiejek");
            this.Register(NumberWordsCurrencies.RON, true, "lej", "leje", "lejów", "бан", "бана", "банов");
            this.Register(NumberWordsCurrencies.JPY, true, "jen", "jenów", "jenów", "siano", "siana", "sian");

            this.Register(NumberWordsCurrencies.RUB, true, "rubel", "ruble", "rubli", "kopiejka", "kopiejki", "kopiejek");
            this.Register(NumberWordsCurrencies.VND, true, "đồng", "đồngów", "đồngów", "xu", "xu", "xu");
            this.Register(NumberWordsCurrencies.INR, true, "rupia", "rupie", "rupii", "pajsa", "pajsa", "pajsa");
        }

        private void Register(NumberWordsCurrencies currency, bool male,
            string seniorOne, string seniorTwo, string seniorFive,
            string juniorOne, string juniorTwo, string juniorFive) {
        CurrencyInfo info;
        info.male=male;
        info.seniorOne=seniorOne;
        info.seniorTwo=seniorTwo;
        info.seniorFive=seniorFive;
        info.juniorOne=juniorOne;
        info.juniorTwo=juniorTwo;
        info.juniorFive=juniorFive;
            this.currencies.Add(currency, info);
        }

        public HybridDictionary Currencies {
            get {
            return this.currencies;
            }
        }
    }
}


/*
лей", "лея", "леев", "бан", "бана", "банов
пятьдесят один лей
пятьдесят два лея
пятьдесят пять леев

пятьдесят один бан
пятьдесят два бана
пятьдесят пять банов

*/
