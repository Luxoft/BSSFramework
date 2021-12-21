using System;
using System.Collections.Generic;
using System.Text;
using Framework.Localization.NumberWords.Interfaces;

namespace Framework.Localization.NumberWords.Dictionaries
{
    public class RuMathOperationWordsDictionary : IMathOperationWordsDictionary
    {
        public string Minus
        {
            get { return "минус "; }
        }

        public string Plus
        {
            get { return "плюс "; }
        }
    }
}
