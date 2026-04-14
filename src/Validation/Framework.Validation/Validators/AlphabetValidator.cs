using CommonFramework;

using Framework.Core;

namespace Framework.Validation.Validators;

public class AlphabetValidator : IPropertyValidator<object, string>
{
    private readonly string alphabet;
    private readonly char[] externalChars;


    public AlphabetValidator(string alphabet, string externalChars = null)
    {
        if (alphabet == null) throw new ArgumentNullException(nameof(alphabet));

        this.alphabet = alphabet;

        this.externalChars = externalChars.EmptyIfNull().ToArray();
    }


    public ValidationResult GetValidationResult(IPropertyValidationContext<object, string> context)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));

        var invalidChars = context.Value.Except(this.alphabet).Except(this.externalChars).Concat();

        return ValidationResult.FromCondition(!invalidChars.Any(),

                                              () => $"The value of {context.GetPropertyName()} property of {context.GetSourceTypeName()} contains invalid chars: {invalidChars}");
    }
}
