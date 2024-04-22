namespace Framework.Validation;

public class AvailableDecimalValidatorAttribute : ClassValidatorAttribute
{
    public override IClassValidator CreateValidator(IServiceProvider serviceProvider)
    {
        return AvailableDecimalValidator.Value;
    }
}
