namespace Framework.Database.EntityFramework.Audit;

public sealed record AuditPropertyMetadata(
    string Name,
    Type PropertyType,
    bool IsKey,
    string ModName,
    bool IsModOnly = false,
    string? NestedPropertyName = null,
    bool IsOwned = false);
