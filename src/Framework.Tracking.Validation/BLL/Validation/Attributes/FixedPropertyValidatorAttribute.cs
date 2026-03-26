using Framework.Tracking.Validation.BLL.Validation.Validators;
using Framework.Validation;

namespace Framework.Tracking.Validation.BLL.Validation.Attributes;

/// <summary>
/// Атрибут для проверки неизменяемости свойства
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class FixedPropertyValidatorAttribute : PropertyValidatorAttribute
{
    public override IPropertyValidator CreateValidator()
    {
        return new FixedPropertyValidator();
    }
}
