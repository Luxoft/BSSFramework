namespace Framework.Validation;

[AttributeUsage(AttributeTargets.Property)]
public class SignValidatorAttribute(SignType expectedPropertyValueSignType) : PropertyValidatorAttribute
{
    public SignType ExpectedPropertyValueSignType { get; } = expectedPropertyValueSignType;

    public override IPropertyValidator CreateValidator() => new SignValidator(this.ExpectedPropertyValueSignType);
}
