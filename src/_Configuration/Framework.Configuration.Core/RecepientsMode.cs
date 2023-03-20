using System.Collections;

namespace Framework.Configuration;

/// <summary>
/// Перечисление принципов комбинации адресатов рассылки
/// </summary>
/// <remarks>
/// Recepients Mode объясняет, как система должна получить рецепиентов в случае, когда оба типа заполнены
/// </remarks>
public enum RecepientsSelectorMode
{
    /// <summary>
    /// Oбъединение "Business Roles" и "Generation"
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
