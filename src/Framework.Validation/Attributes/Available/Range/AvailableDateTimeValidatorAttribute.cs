namespace Framework.Validation;

public class AvailableDateTimeValidatorAttribute : ClassValidatorAttribute
{
    public override IClassValidator CreateValidator()
    {
        return AvailableDateTimeValidator.Value;
    }
}
