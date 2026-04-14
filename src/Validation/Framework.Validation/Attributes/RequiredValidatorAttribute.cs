using Framework.Restriction;
using Framework.Validation.Attributes._Base;
using Framework.Validation.Validators;

namespace Framework.Validation.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class RequiredValidatorAttribute : PropertyValidatorAttribute
{
    public override IPropertyValidator CreateValidator() => new RequiredValidator(this.Mode);

    public RequiredMode Mode { get; set; }
}
