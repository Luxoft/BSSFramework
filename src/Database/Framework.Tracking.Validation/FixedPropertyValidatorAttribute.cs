using Framework.Validation.Attributes;
using Framework.Validation.Validators;

namespace Framework.Tracking.Validation;

/// <summary>
/// Attribute for checking property immutability
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class FixedPropertyValidatorAttribute : PropertyValidatorAttribute
{
    public override IPropertyValidator CreateValidator() => new FixedPropertyValidator();
}
