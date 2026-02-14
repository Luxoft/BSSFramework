using Framework.Validation;

namespace Framework.DomainDriven.Tracking.LegacyValidators;

/// <summary>
/// Атрибут для проверки неизменяемости свойства
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class FixedPropertyValidatorAttribute : PropertyValidatorAttribute
{
    public override IPropertyValidator CreateValidator(PropertyInfo property, IServiceProvider serviceProvider)
    {
        return new FixedPropertyValidator();
    }
}
