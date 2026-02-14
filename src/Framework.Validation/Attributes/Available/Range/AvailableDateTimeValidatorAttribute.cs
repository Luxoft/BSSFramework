namespace Framework.Validation;

public class AvailableDateTimeValidatorAttribute : ClassValidatorAttribute
{
    public override IClassValidator CreateValidator(Type type, IServiceProvider serviceProvider)
    {
        return AvailableDateTimeValidator.Value;
    }
}
