using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Localization.NumberWords.Interfaces
{
    public interface INumberWordsDictionary
    {
        ICurrencyInfoDictionary CurrencyInfoDictionary { get; }
        IDecimalExponentsDictionary DecimalExponentsDictionary { get; }
        IDecimalNumberWordsDictionary DecimalNumberWordsDictionary { get; }
        IMathOperationWordsDictionary MathOperationDictinary { get; }
    }
}
