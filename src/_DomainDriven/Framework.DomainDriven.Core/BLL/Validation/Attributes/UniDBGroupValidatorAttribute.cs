using System;

using Framework.Validation;

namespace Framework.DomainDriven.BLL;

/// <summary>
/// Атрибут проверки уникальности через запрос к БД.
/// </summary>
/// <seealso cref="Framework.Validation.ClassValidatorAttribute" />
[AttributeUsage(AttributeTargets.Class)]
public class UniDBGroupValidatorAttribute : ClassValidatorAttribute
{
    /// <summary>
    ///     Возвращает строковый ключ группы.
    /// </summary>
    /// <value>
    ///     Строковый ключ группы.
    /// </value>
    public string GroupKey { get; set; }

    /// <summary>
    ///     Получает или возвращает флаг, указывающий на необходимость проверки уникальности путем запроса к БД.
    /// </summary>
    /// <value>
    ///     <c>true</c> если еобходимо проверять уникальность путем запроса к БД; в противном случае, <c>false</c>.
    /// </value>
    public bool UseDbEvaluation { get; set; }

    /// <inheritdoc />
    public override IClassValidator CreateValidator()
    {
        return new UniqueGroupDatabaseValidator(this.GroupKey);
    }
}
