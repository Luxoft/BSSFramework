using Framework.Core;

namespace Framework.Validation;

public class AlphabetValidator : IPropertyValidator<object, string>
{
    private readonly string _alphabet;
    private readonly char[] _externalChars;


    public AlphabetValidator(string alphabet, string externalChars = null)
    {
        if (alphabet == null) throw new ArgumentNullException(nameof(alphabet));

        this._alphabet = alphabet;

        this._externalChars = externalChars.EmptyIfNull().ToArray();
    }


    public ValidationResult GetValidationResult(IPropertyValidationContext<object, string> context)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));

        var invalidChars = context.Value.Except(this._alphabet).Except(this._externalChars).Concat();

        return ValidationResult.FromCondition(!invalidChars.Any(),

                                              () => $"The value of {context.GetPropertyName()} property of {context.GetSourceTypeName()} contains invalid chars: {invalidChars}");
    }
}
