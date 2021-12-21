using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Localization.NumberWords.Interfaces
{
    public interface IDecimalExponentsDictionary
    {
        string[] Exponent(DecimalExponentsEnum exponents);
    }
}
