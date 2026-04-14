using Framework.Validation.Validators;

namespace Framework.Validation;

public class AvailablePeriodValidatorAttribute : ClassValidatorAttribute
{
    public override IClassValidator CreateValidator() => AvailablePeriodValidator.Value;
}
