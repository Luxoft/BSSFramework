using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Localization.NumberWords.Interfaces
{
    public interface IDecimalNumberWordsDictionary
    {
        bool Male { get; set; }
        string[] Hunds { get; }
        string[] Tens { get; }
        string[] Frac { get; }
    }
}
