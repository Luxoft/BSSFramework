namespace Framework.Authorization.Domain;

/// <summary>
/// Перечень действий, который можно делать c объектом
/// </summary>
[Flags]
public enum AuthorizationOperationContext
{
    Create = 1,

    Edit = 2,

    Save = 4,

    SavePrincipal = 8,

    All = Create | Edit | Save | SavePrincipal
}
