namespace Framework.Validation;

public class AvailableDateTimeValidatorAttribute : ClassValidatorAttribute
{
    public override IClassValidator CreateValidator(IServiceProvider serviceProvider)
    {
        return AvailableDateTimeValidator.Value;
    }
}
