namespace Framework.Validation;

public class AvailableDecimalValidatorAttribute : ClassValidatorAttribute
{
    public override IClassValidator CreateValidator() => AvailableDecimalValidator.Value;
}
