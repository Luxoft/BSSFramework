using System.Linq;

using Framework.Core;

namespace Framework.Validation;

public class EnglishAlphabetValidator : AlphabetValidator
{
    private static readonly string EnglishAlphabet =
            Enumerable.Range('a', 'z').Concat(Enumerable.Range ('A', 'Z')).Select(v => (char) v).Concat();


    public EnglishAlphabetValidator (string externalChars)
            : base (EnglishAlphabet, externalChars)
    {

    }
}
