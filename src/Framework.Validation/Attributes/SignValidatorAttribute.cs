using System.Reflection;

namespace Framework.Validation;

[AttributeUsage(AttributeTargets.Property)]
public class SignValidatorAttribute : PropertyValidatorAttribute
{
    public SignValidatorAttribute(SignType expectedPropertyValueSignType)
    {
        this.ExpectedPropertyValueSignType = expectedPropertyValueSignType;
    }


    public SignType ExpectedPropertyValueSignType { get; }

    public override IPropertyValidator CreateValidator(PropertyInfo property, IServiceProvider serviceProvider)
    {
        return new SignValidator(this.ExpectedPropertyValueSignType);
    }
}
