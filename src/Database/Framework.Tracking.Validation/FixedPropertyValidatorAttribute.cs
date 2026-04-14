using Framework.Validation.Attributes;
using Framework.Validation.Validators;

namespace Framework.Tracking.Validation;

/// <summary>
/// Атрибут для проверки неизменяемости свойства
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class FixedPropertyValidatorAttribute : PropertyValidatorAttribute
{
    public override IPropertyValidator CreateValidator() => new FixedPropertyValidator();
}
