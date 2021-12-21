using System;
using System.Collections.Generic;
using System.Text;
using Framework.Localization.NumberWords.Interfaces;

namespace Framework.Localization.NumberWords.Dictionaries
{
    class NumberWordsDictionary : INumberWordsDictionary
    {
        private ICurrencyInfoDictionary currencyInfoDictionary;
        private IDecimalExponentsDictionary decimalExponentsDictionary;
        private IDecimalNumberWordsDictionary decimalNumberWordsDictionary;
        private IMathOperationWordsDictionary mathOperationDictinary;

        public NumberWordsDictionary(
            ICurrencyInfoDictionary currencyInfoDictionary,
            IDecimalExponentsDictionary decimalExponentsDictionary,
            IDecimalNumberWordsDictionary decimalNumberWordsDictionary,
            IMathOperationWordsDictionary mathOperationDictinary)
        {
            this.currencyInfoDictionary = currencyInfoDictionary;
            this.decimalExponentsDictionary = decimalExponentsDictionary;
            this.decimalNumberWordsDictionary = decimalNumberWordsDictionary;
            this.mathOperationDictinary = mathOperationDictinary;
        }

        public ICurrencyInfoDictionary CurrencyInfoDictionary
        {
            get { return this.currencyInfoDictionary; }
        }

        public IDecimalExponentsDictionary DecimalExponentsDictionary
        {
            get { return this.decimalExponentsDictionary; }
        }

        public IDecimalNumberWordsDictionary DecimalNumberWordsDictionary
        {
            get { return this.decimalNumberWordsDictionary; }
        }

        public IMathOperationWordsDictionary MathOperationDictinary
        {
            get { return this.mathOperationDictinary; }
        }
    }
}
