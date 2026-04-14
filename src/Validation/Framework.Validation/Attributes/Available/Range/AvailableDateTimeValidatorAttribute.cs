using Framework.Validation.Validators;
using Framework.Validation.Validators.DynamicClass.Available;

namespace Framework.Validation.Attributes.Available.Range;

public class AvailableDateTimeValidatorAttribute : ClassValidatorAttribute
{
    public override IClassValidator CreateValidator() => AvailableDateTimeValidator.Value;
}
