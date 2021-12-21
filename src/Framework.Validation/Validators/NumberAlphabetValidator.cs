namespace Framework.Validation
{
    public class NumberAlphabetValidator : AlphabetValidator
    {
        private const string NumberAlphabet = "0123456789";

        public NumberAlphabetValidator(string externalChars)
            : base(NumberAlphabet, externalChars)
        {

        }
    }
}