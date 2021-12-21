using System;
using System.Collections.Generic;
using System.Text;
using Framework.Localization.NumberWords.Interfaces;

namespace Framework.Localization.NumberWords.Dictionaries
{
    public class EnMathOperationWordsDictionary : IMathOperationWordsDictionary
    {
        public string Minus
        {
            get { return "minus "; }
        }

        public string Plus
        {
            get { return "plus "; }
        }
    }
}
