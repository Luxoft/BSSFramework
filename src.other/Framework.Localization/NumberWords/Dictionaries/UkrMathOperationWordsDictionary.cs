using Framework.Localization.NumberWords.Interfaces;

namespace Framework.Localization.NumberWords.Dictionaries
{
    class UkrMathOperationWordsDictionary : IMathOperationWordsDictionary
    {
        public string Minus
        {
            get { return "мінус "; }
        }

        public string Plus
        {
            get { return "плюс "; }
        }
    }
}
