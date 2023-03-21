using System;

using Framework.Restriction;

namespace Framework.Validation;

[AttributeUsage(AttributeTargets.Property)]
public class RequiredValidatorAttribute : PropertyValidatorAttribute
{
    public override IPropertyValidator CreateValidator()
    {
        return new RequiredValidator(this.Mode);
    }

    public RequiredMode Mode { get; set; }
}
