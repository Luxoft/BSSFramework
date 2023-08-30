namespace Framework.Validation;

public class AvailablePeriodValidatorAttribute : ClassValidatorAttribute
{
    public override IClassValidator CreateValidator()
    {
        return AvailablePeriodValidator.Value;
    }
}
