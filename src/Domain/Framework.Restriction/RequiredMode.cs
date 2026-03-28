namespace Framework.Restriction;

/// <summary>
/// Дополнительные режимы валидации
/// </summary>
public enum RequiredMode
{
    Default,

    /// <summary>
    /// Разрешает пустую строку, идёт проверка только на null-значение
    /// </summary>
    AllowEmptyString,

    /// <summary>
    /// Требует, чтобы период был строго закрыт, т.е. Period.EndDate != null
    /// </summary>
    ClosedPeriodEndDate
}
