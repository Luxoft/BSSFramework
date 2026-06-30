using Anch.Core;

using Framework.Core;

namespace Framework.Validation.Validators;

public class AlphabetValidator(string alphabet, string? externalChars = null) : IPropertyValidator<object, string>
{
    private readonly string alphabet = alphabet ?? throw new ArgumentNullException(nameof(alphabet));
    private readonly char[] externalChars = externalChars.EmptyIfNull().ToArray();

    public ValidationResult GetValidationResult(IPropertyValidationContext<object, string> context)
    {
        if (context is null) throw new ArgumentNullException(nameof(context));

        var invalidChars = context.Value.Except(this.alphabet).Except(this.externalChars).Concat();

        return ValidationResult.FromCondition(!invalidChars.Any(),

                                              () => $"The value of {context.GetPropertyName()} property of {context.GetSourceTypeName()} contains invalid chars: {invalidChars}");
    }
}

