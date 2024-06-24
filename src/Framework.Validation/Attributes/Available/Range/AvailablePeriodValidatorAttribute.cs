namespace Framework.Validation;

public class AvailablePeriodValidatorAttribute : ClassValidatorAttribute
{
    public override IClassValidator CreateValidator(IServiceProvider serviceProvider)
    {
        return AvailablePeriodValidator.Value;
    }
}
