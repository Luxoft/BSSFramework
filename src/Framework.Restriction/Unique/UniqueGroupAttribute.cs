namespace Framework.Restriction;

/// <summary>
///     Проверка на уникальность по набору полей. Генерация констрейнта на таблице. Применяется как на сам объект, так и на
///     коллекции в нём.
/// </summary>
/// <seealso cref="System.Attribute" />
/// <seealso cref="Framework.Restriction.IUniqueAttribute" />
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, AllowMultiple = true)]
public class UniqueGroupAttribute : Attribute, IUniqueAttribute
{
    /// <summary>
    ///     Создаёт экземпляр класса <see cref="UniqueGroupAttribute" />.
    /// </summary>
    public UniqueGroupAttribute()
    {
    }

    /// <summary>
    ///     Создаёт экземпляр класса <see cref="UniqueGroupAttribute" />.
    /// </summary>
    /// <param name="key">Строковый ключ группы.</param>
    public UniqueGroupAttribute(string key)
    {
        this.Key = key;
    }

    /// <summary>
    ///     Возвращает строковый ключ группы.
    /// </summary>
    /// <value>
    ///     Строковый ключ группы.
    /// </value>
    public string Key { get; }

    /// <summary>
    ///     Получает или возвращает флаг, указывающий на необходимость проверки уникальности путем запроса к БД.
    /// </summary>
    /// <value>
    ///     <c>true</c> если еобходимо проверять уникальность путем запроса к БД; в противном случае, <c>false</c>.
    /// </value>
    public bool UseDbEvaluation { get; set; }
}
