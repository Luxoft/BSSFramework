using Framework.Localization.NumberWords.Interfaces;

namespace Framework.Localization.NumberWords.Dictionaries
{
    public class PlMathOperationWordsDictionary: IMathOperationWordsDictionary
    {
        public string Minus {
            get {
            return "minus ";
            }
        }

        public string Plus {
            get {
            return "plus ";
            }
        }
    }
}
