using Framework.Validation.Validators;
using Framework.Validation.Validators.DynamicClass;

namespace Framework.Validation.Attributes.Available;

/// <summary>
/// Ограничение максимальной длинны строк, применяемое для всех строковых свойств, на которых нет явного аттрибута ограничения Framework.Restriction.MaxLengthAttribute
/// </summary>
public class DefaultStringMaxLengthValidatorAttribute : ClassValidatorAttribute
{
    public override IClassValidator CreateValidator() => DefaultStringMaxLengthValidator.Value;
}
