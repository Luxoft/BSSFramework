using Framework.Localization.NumberWords.Enums;
using Framework.Localization.NumberWords.Interfaces;
using System.Collections.Specialized;

namespace Framework.Localization.NumberWords.Dictionaries
{
    public class UkrCurrencyInfoDictionary : ICurrencyInfoDictionary
    {
        readonly HybridDictionary currencies = new HybridDictionary();

        public UkrCurrencyInfoDictionary()
        {
            this.Register(NumberWordsCurrencies.RUR, true, "рубль", "рублі", "рублів", "копійка", "копiйки", "копiйок");
            this.Register(NumberWordsCurrencies.EUR, true, "євро", "євро", "євро", "євроцент", "євроценти", "євроцентiв");
            this.Register(NumberWordsCurrencies.USD, true, "долар", "долари", "доларів", "цент", "центи", "центів");
            this.Register(NumberWordsCurrencies.GBP, true, "фунт", "фунти", "фунтiв", "пенс", "пенси", "пенсiв");
            this.Register(NumberWordsCurrencies.CHF, true, "франк", "франки", "франкiв", "сантим", "сантими", "сантимiв");
            this.Register(NumberWordsCurrencies.CAD, true, "долар", "долари", "доларів", "цент", "центи", "центів");
            this.Register(NumberWordsCurrencies.UAH, false, "гривня", "гривнi", "гривень", "копiйка", "копiйки", "копiйок");
            this.Register(NumberWordsCurrencies.RON, true, "лей", "леи", "леів", "бан", "бани", "банів");
            this.Register(NumberWordsCurrencies.JPY, true, "ієна", "ієни", "ієн", "сіна", "сени", "сен");
            this.Register(NumberWordsCurrencies.PLN, true, "злотий", "злотих", "злотих", "гріш", "гроша", "грошів");

            this.Register(NumberWordsCurrencies.RUB, true, "рубль", "рублі", "рублів", "копійка", "копiйки", "копiйок");
            this.Register(NumberWordsCurrencies.VND, true, "донгів", "донгів", "донгів", "су", "су", "су");
            this.Register(NumberWordsCurrencies.INR, true, "рупия", "рупії", "рупій", "пайса", "пайса", "пайса");
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