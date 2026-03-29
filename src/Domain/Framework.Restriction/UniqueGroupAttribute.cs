namespace Framework.Restriction;

/// <summary>
///     Проверка на уникальность по набору полей. Генерация констрейнта на таблице. Применяется как на сам объект, так и на
///     коллекции в нём.
/// </summary>
/// <seealso cref="System.Attribute" />
/// <seealso cref="Framework.Restriction.IUniqueAttribute" />
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, AllowMultiple = true)]
public class UniqueGroupAttribute(string? key = null) : Attribute, IUniqueAttribute
{
    /// <summary>
    ///     Возвращает строковый ключ группы.
    /// </summary>
    /// <value>
    ///     Строковый ключ группы.
    /// </value>
    public string? Key { get; } = key;

    /// <summary>
    ///     Получает или возвращает флаг, указывающий на необходимость проверки уникальности путем запроса к БД.
    /// </summary>
    /// <value>
    ///     <c>true</c> если необходимо проверять уникальность путем запроса к БД; в противном случае, <c>false</c>.
    /// </value>
    public bool UseDbEvaluation { get; set; }
}
