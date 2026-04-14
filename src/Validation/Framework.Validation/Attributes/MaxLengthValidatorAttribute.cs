using Framework.Validation.Attributes._Base;
using Framework.Validation.Validators;

namespace Framework.Validation.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class MaxLengthValidatorAttribute : PropertyValidatorAttribute
{
    public int MaxLength { get; set; } = int.MaxValue;


    public override IPropertyValidator CreateValidator() => new MaxLengthValidator(this.MaxLength);
}
