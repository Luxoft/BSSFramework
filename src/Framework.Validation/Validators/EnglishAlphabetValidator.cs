using Framework.Core;

namespace Framework.Validation;

public class EnglishAlphabetValidator(string externalChars) : AlphabetValidator(EnglishAlphabet, externalChars)
{
    private static readonly string EnglishAlphabet =
            Enumerable.Range('a', 'z').Concat(Enumerable.Range ('A', 'Z')).Select(v => (char) v).Concat();
}
