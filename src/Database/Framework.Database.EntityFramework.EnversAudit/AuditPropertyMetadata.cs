namespace Framework.Database.EntityFramework.EnversAudit;

public sealed record AuditPropertyMetadata(
    string Name,
    Type PropertyType,
    bool IsKey,
    string ModName,
    bool IsModOnly = false,
    string? NestedPropertyName = null,
    bool IsOwned = false,
    Type? InverseReferenceEntityType = null);
