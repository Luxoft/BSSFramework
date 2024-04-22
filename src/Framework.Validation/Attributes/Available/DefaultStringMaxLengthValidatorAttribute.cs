namespace Framework.Validation;

/// <summary>
/// Ограничение максимальной длинны строк, применяемое для всех строковых свойств, на которых нет явного аттрибута ограничения Framework.Restriction.MaxLengthAttribute
/// </summary>
public class DefaultStringMaxLengthValidatorAttribute : ClassValidatorAttribute
{
    public override IClassValidator CreateValidator(IServiceProvider serviceProvider)
    {
        return DefaultStringMaxLengthValidator.Value;
    }
}
