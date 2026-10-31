using Framework.Validation.Validators;
using Framework.Validation.Validators.DynamicClass;

namespace Framework.Validation.Attributes.Available;

/// <summary>
/// Maximum string length limit applied to all string properties that have no explicit Framework.Restriction.MaxLengthAttribute restriction attribute
/// </summary>
public class DefaultStringMaxLengthValidatorAttribute : ClassValidatorAttribute
{
    public override IClassValidator CreateValidator() => DefaultStringMaxLengthValidator.Value;
}
