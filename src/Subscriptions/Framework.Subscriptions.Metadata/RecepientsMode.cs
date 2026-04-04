namespace Framework.Subscriptions.Metadata;

/// <summary>
/// Перечисление принципов комбинации адресатов рассылки
/// </summary>
/// <remarks>
/// Recipients Mode объясняет, как система должна получить рецепиентов в случае, когда оба типа заполнены
/// </remarks>
public enum RecipientsSelectorMode
{
    /// <summary>
    /// Объединение "Business Roles" и "Generation"
    /// </summary>
    Union,

    /// <summary>
    /// "Business Roles" исключает "Generation"
    /// </summary>
    RolesExceptGeneration,

    /// <summary>
    /// "Generation" исключает "Business Roles"
    /// </summary>
    GenerationExceptRoles,

    /// <summary>
    /// Пересечение "Business Roles" и "Generation"
    /// </summary>
    Intersect
}
