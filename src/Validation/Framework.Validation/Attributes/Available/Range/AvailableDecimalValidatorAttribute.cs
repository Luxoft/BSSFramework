using Framework.Validation.Validators;
using Framework.Validation.Validators.DynamicClass.Available;

namespace Framework.Validation.Attributes.Available.Range;

public class AvailableDecimalValidatorAttribute : ClassValidatorAttribute
{
    public override IClassValidator CreateValidator() => AvailableDecimalValidator.Value;
}
