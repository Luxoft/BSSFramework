namespace Framework.Validation;

public class NumberAlphabetValidator(string externalChars) : AlphabetValidator(NumberAlphabet, externalChars)
{
    private const string NumberAlphabet = "0123456789";
}
