using Framework.Validation.Validators;

namespace Framework.Validation;

public class AvailableDateTimeValidatorAttribute : ClassValidatorAttribute
{
    public override IClassValidator CreateValidator() => AvailableDateTimeValidator.Value;
}
