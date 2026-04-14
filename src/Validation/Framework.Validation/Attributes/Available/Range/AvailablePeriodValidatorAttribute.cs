using Framework.Validation.Validators;
using Framework.Validation.Validators.DynamicClass.Available;

namespace Framework.Validation.Attributes.Available.Range;

public class AvailablePeriodValidatorAttribute : ClassValidatorAttribute
{
    public override IClassValidator CreateValidator() => AvailablePeriodValidator.Value;
}
